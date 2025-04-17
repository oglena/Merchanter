// See https://aka.ms/new-console-template for more information
using Merchanter;
using MerchanterServer;
using System.Reflection;
using Customer = Merchanter.Classes.Customer;
using DbHelper = Merchanter.DbHelper;
using Helper = Merchanter.Helper;

var now = DateTime.Now;
const string newline = "\r\n";
bool first_run = true;
PrintConsole("Press Ctrl+C to exit.");
int customer_id = 0;

#region Customer Params
if (args.Length == 0) {
START:
    PrintConsole("Please provide Merchanter Customer ID:");
    string? CID = Console.ReadLine();
    if (CID != null && CID != "0") {
        _ = int.TryParse(CID, out customer_id);
    }
    else goto START;
}
else {
    if (args[0] != null) {
        _ = int.TryParse(args[0], out customer_id);
    }
    else {
        PrintConsole("Customer parameter error -1", ConsoleColor.Red);
        PrintConsole("-1 exited");
        Thread.Sleep(2000);
        return -1;
    }
}

if (customer_id <= 0) {
    PrintConsole("Customer parameter error -1", ConsoleColor.Red);
    PrintConsole("-1 exited");
    Thread.Sleep(2000);
    return -1;
}
#endregion

#region Helper Instance
if (Constants.Server == null || Constants.User == null || Constants.Password == null || Constants.Database == null || Constants.Port <= 0) {
    PrintConsole("Database connection parameters are not properly set from App.config file.", ConsoleColor.Red);
    PrintConsole("-1 exited");
    Thread.Sleep(2000);
    return -1;
}
DbHelper db_helper = new(Constants.Server, Constants.User, Constants.Password, Constants.Database, Constants.Port);
PrintConsole("Merchanter Sync | Ceres Software & Consultancy" + " Version: " + Assembly.GetExecutingAssembly().GetName().Version?.ToString() + " DB:[" + db_helper.Database + "]", ConsoleColor.Green);
db_helper.ErrorOccured += (sender, e) => { Console.WriteLine(e); db_helper.LogToServer("WTF", "helper_error", e, customer_id, "helper_instance"); };

string thread_id = Guid.NewGuid().ToString();
Thread.CurrentThread.Name = thread_id + ",main";
PrintConsole("Thread Started " + thread_id, ConsoleColor.Green);
if (!db_helper.LogToServer(thread_id, "merchanter", "started " + thread_id, customer_id, "main_thread")) {
    PrintConsole("Merchanter Database error.", ConsoleColor.Red);
    PrintConsole("Thread Ended." + thread_id, ConsoleColor.Red);
    //GMail.Send( Constants.mail_sender, Constants.mail_password, Constants.mail_sender_name, Constants.mail_to,
    //    Assembly.GetCallingAssembly().GetName().Name + "Error  //customer:" + customer_id.ToString(),
    //    "ERROR" + newline + "CustomerID: " + customer_id.ToString() + ". Ceres Database Not Found" + newline + "exit -99" );
    PrintConsole("-99 exited");
    Console.ReadLine();
    return -99;
}
else {
    db_helper.invoice = new(db_helper.Server, db_helper.User, db_helper.Password, db_helper.Database, db_helper.Port);
    db_helper.xml = new(db_helper.Server, db_helper.User, db_helper.Password, db_helper.Database, db_helper.Port);
    db_helper.notification = new(db_helper.Server, db_helper.User, db_helper.Password, db_helper.Database, db_helper.Port);
}
#if WINDOWS
PrintConsole("Welcome! " + System.Security.Principal.WindowsIdentity.GetCurrent()?.Name + " Program starting for " + customer_id, ConsoleColor.Blue);
#else
PrintConsole("Welcome! Program starting for " + customer_id, ConsoleColor.Blue);
#endif
AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
#endregion

while (true) {
    #region Load Customer & Check License
    Customer? customer = db_helper.GetCustomer(customer_id);

    if (customer == null) {
        PrintConsole("Customer not found. Exiting.", ConsoleColor.Red);
        PrintConsole("Thread Ended." + thread_id, ConsoleColor.Red);
        db_helper.LogToServer(thread_id, "error", "user not found", customer_id, "customer");
        db_helper.LogToServer(thread_id, "thread", "ended " + thread_id, customer_id, "customer");
        //GMail.Send( Constants.mail_sender, Constants.mail_password, Constants.mail_sender_name, Constants.mail_to,
        //    Assembly.GetCallingAssembly().GetName().Name + "Error  //customer:" + customer_id.ToString(),
        //    "ERROR" + newline + "CustomerID: " + customer_id.ToString() + ". User Not Found." + newline + "exit -2" );
        PrintConsole("Thread will sleep 1h!"); Thread.Sleep(1000 * 60 * 60); //1h
        PrintConsole("-2 exited");
        Thread.Sleep(2000);
        return -2;
    }

    if (!customer.status) {
        PrintConsole(customer_id + "-ID license error.", ConsoleColor.Red);
        PrintConsole("Thread will sleep 10m!"); Thread.Sleep(1000 * 60 * 10); //10m
        continue;
    }
    #endregion

    #region Helper Settings
    try {
        db_helper.LoadSettings(customer_id);

        if (Helper.global == null) {
            db_helper.LogToServer(thread_id, "friendly_error", "Settings could not load.", customer_id, "helper_settings");
            PrintConsole("Settings could not load.", ConsoleColor.Red);
            PrintConsole("Thread will sleep 10m!"); Thread.Sleep(1000 * 60 * 10); //10m
            continue;
        }
    }
    catch (Exception ex) {
        db_helper.LogToServer(thread_id, "friendly_error", "Settings could not load.", customer_id, "helper_settings");
        PrintConsole("Settings could not load." + newline + ex.ToString(), ConsoleColor.Red);
        PrintConsole("Thread will sleep 10m!"); Thread.Sleep(1000 * 60 * 10); //10m
        continue;
    }
    #endregion

    #region First Run
    if (first_run) {
        first_run = false;
        if (db_helper.SetProductSyncWorking(customer_id, false) && db_helper.SetOrderSyncWorking(customer_id, false) && db_helper.SetNotificationSyncWorking(customer_id, false) && db_helper.SetXmlSyncWorking(customer_id, false) && db_helper.SetInvoiceSyncWorking(customer_id, false))
            PrintConsole(customer_id + "-ID sync statuses reset for first run.", ConsoleColor.Blue);

    }
    #endregion

    #region Decision to Work
    if (customer.product_sync_status && !customer.is_productsync_working) {
        if (customer.last_product_sync_date != null)
            if (customer.last_product_sync_date > DateTime.Now.AddSeconds(customer.product_sync_timer * -1))
                customer.product_sync_status = false;
    }
    if (customer.order_sync_status && !customer.is_ordersync_working) {
        if (customer.last_order_sync_date != null)
            if (customer.last_order_sync_date > DateTime.Now.AddSeconds(customer.order_sync_timer * -1))
                customer.order_sync_status = false;
    }
    if (customer.xml_sync_status && !customer.is_xmlsync_working) {
        if (customer.last_xml_sync_date != null)
            if (customer.last_xml_sync_date > DateTime.Now.AddSeconds(customer.xml_sync_timer * -1))
                customer.xml_sync_status = false;
    }
    if (customer.invoice_sync_status && !customer.is_invoicesync_working) {
        if (customer.last_invoice_sync_date != null)
            if (customer.last_invoice_sync_date > DateTime.Now.AddSeconds(customer.invoice_sync_timer * -1))
                customer.invoice_sync_status = false;
    }
    if (customer.notification_sync_status && !customer.is_notificationsync_working) {
        if (customer.last_notification_sync_date != null)
            if (customer.last_notification_sync_date > DateTime.Now.AddSeconds(customer.notification_sync_timer * -1))
                customer.notification_sync_status = false;
    }
    if (!customer.product_sync_status && !customer.order_sync_status && !customer.xml_sync_status && !customer.invoice_sync_status && !customer.notification_sync_status) {
        //PrintConsole( "Thread will sleep 5secs!" );
        Thread.Sleep(5 * 1000); //5secs
        continue;
    }
    #endregion

    try {
        MainLoop main_loop = new(thread_id, customer, db_helper);
        if (Helper.global != null && main_loop.DoWork()) {
            if (customer.product_sync_status && !customer.is_productsync_working) {
                PrintConsole("PRODUCT SYNC COMPLETED", ConsoleColor.Blue);
            }
            if (customer.order_sync_status && !customer.is_ordersync_working) {
                PrintConsole("ORDER SYNC COMPLETED", ConsoleColor.Blue);
            }
            if (customer.xml_sync_status && !customer.is_xmlsync_working) {
                PrintConsole("XML SYNC COMPLETED", ConsoleColor.Blue);
            }
            if (customer.invoice_sync_status && !customer.is_invoicesync_working) {
                PrintConsole("INVOICE SYNC COMPLETED", ConsoleColor.Blue);
            }
            if (customer.notification_sync_status && !customer.is_notificationsync_working) {
                PrintConsole("NOTIFICATION SYNC COMPLETED", ConsoleColor.Blue);
            }
        }
    }
    catch (Exception _ex) {
        db_helper.LogToServer(thread_id, "error", _ex.Message + newline + _ex.ToString(), customer_id, "general");
        //GMail.Send( Constants.mail_sender, Constants.mail_password, Constants.mail_sender_name, Constants.mail_to,
        //    Assembly.GetCallingAssembly().GetName().Name + "-Error  //customer:" + customer_id.ToString(),
        //    "ERROR" + newline + "CustomerID: " + customer_id.ToString() + ". " + _ex.Message + newline + _ex.ToString() + newline + "exit -2" );
        PrintConsole(_ex.Message + newline + _ex.ToString(), ConsoleColor.Red);
        PrintConsole("Thread will sleep 1m!"); Thread.Sleep(1000 * 60 * 1); //1m
        continue;
    }
    finally {

    }
}

void CurrentDomain_ProcessExit(object? sender, EventArgs e) {
    if (db_helper.LogToServer(thread_id, "merchanter", "ended " + thread_id, customer_id, "main_thread")) {
        PrintConsole("exit success", ConsoleColor.Cyan);
        Thread.Sleep(2000);
    }
}

void PrintConsole(string value, ConsoleColor _color = ConsoleColor.White) {
    Console.ForegroundColor = _color;
    Console.WriteLine(value);
    Console.ResetColor();
}