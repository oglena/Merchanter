// See https://aka.ms/new-console-template for more information
using Merchanter;
using Merchanter.Classes;
using MerchanterServer;
using System.Diagnostics;
using System.Reflection;
using Customer = Merchanter.Classes.Customer;
using DbHelper = Merchanter.DbHelper;
using Helper = Merchanter.Helper;

var now = DateTime.Now;
const string newline = "\r\n";
bool first_run = true;
Console.WriteLine( "Press Ctrl+C to exit." );
Console.Beep();
int customer_id;

#region Customer Params
if( args != null && args.Length > 0 ) {
    if( args[ 0 ] != null ) {
        _ = int.TryParse( args[ 0 ], out customer_id );
    }
    else {
        Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Customer parameter error -1" );
        Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Customer parameter error -1" );
        return -1;
    }
}
else {
    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Customer parameter error -1" );
    Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Customer parameter error -1" );
    return -1;
}

if( customer_id <= 0 ) {
    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Customer parameter error -1" );
    Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Customer parameter error -1" );
    return -1;
}
#endregion

#region Helper Instance
DbHelper db_helper = new( Constants.Server, Constants.User, Constants.Password, Constants.Database, Constants.Port );
Console.WriteLine( "[" + now.ToString() + "] " + "Merchanter Sync | Ceres Software & Consultancy" + " Version: " + Assembly.GetExecutingAssembly().GetName().Version?.ToString() + " DB:[" + db_helper.Database + "]" );
db_helper.ErrorOccured += ( sender, e ) => { Console.WriteLine( e ); db_helper.LogToServer( "WTF", "helper_error", e, customer_id, "helper_instance" ); };

string thread_id = Guid.NewGuid().ToString();
Thread.CurrentThread.Name = thread_id + ",main";
Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Thread Started " + thread_id );
Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Thread Started " + thread_id );
if( !db_helper.LogToServer( thread_id, "merchanter", "started " + thread_id, customer_id, "main_thread" ) ) {
    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Ceres Database error." );
    Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Ceres Database error." );
    Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Thread Ended " + thread_id );
    Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Thread Ended " + thread_id );
    //GMail.Send( Constants.mail_sender, Constants.mail_password, Constants.mail_sender_name, Constants.mail_to,
    //    Assembly.GetCallingAssembly().GetName().Name + "Error  //customer:" + customer_id.ToString(),
    //    "ERROR" + newline + "CustomerID: " + customer_id.ToString() + ". Ceres Database Not Found" + newline + "exit -99" );
    Console.WriteLine( "-99 exited" );
    return -99;
}
else {
    db_helper.invoice = new( db_helper.Server, db_helper.User, db_helper.Password, db_helper.Database, db_helper.Port );
    db_helper.xml = new( db_helper.Server, db_helper.User, db_helper.Password, db_helper.Database, db_helper.Port );
    db_helper.notification = new( db_helper.Server, db_helper.User, db_helper.Password, db_helper.Database, db_helper.Port );
}

AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
#endregion

while( true ) {
    #region Load Customer & Check License
    Customer customer = db_helper.GetCustomer( customer_id );

    if( customer == null ) {
        Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Customer not found. Exiting." );
        Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Customer not found. Exiting." );
        db_helper.LogToServer( thread_id, "error", "user not found", customer_id, "customer" );
        Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Thread Ended " + thread_id );
        Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Thread Ended " + thread_id );
        db_helper.LogToServer( thread_id, "thread", "ended " + thread_id, customer_id, "customer" );
        //GMail.Send( Constants.mail_sender, Constants.mail_password, Constants.mail_sender_name, Constants.mail_to,
        //    Assembly.GetCallingAssembly().GetName().Name + "Error  //customer:" + customer_id.ToString(),
        //    "ERROR" + newline + "CustomerID: " + customer_id.ToString() + ". User Not Found." + newline + "exit -2" );
        Console.WriteLine( "Thread will sleep 10m!" );
        Thread.Sleep( 1000 * 60 * 10 ); //10m
        return -2;
    }

    if( !customer.status ) {
        Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + customer_id + "-ID license error." );
        Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + customer_id + "-ID license error." );
        Console.WriteLine( "Thread will sleep 1h!" );
        Thread.Sleep( 1000 * 60 * 60 ); //1h
        continue;
    }
    #endregion

    #region Helper Settings
    try {
        db_helper.LoadSettings( customer_id );

        if( Helper.global == null ) {
            db_helper.LogToServer( thread_id, "friendly_error", "Settings could not load.", customer_id, "helper_settings" );
            Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Settings could not load." );
            Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Settings could not load." );
            Console.WriteLine( "Thread will sleep 10m!" );
            Thread.Sleep( 1000 * 60 * 10 ); //10m
            continue;
        }
    }
    catch( Exception ex ) {
        db_helper.LogToServer( thread_id, "friendly_error", "Settings could not load.", customer_id, "helper_settings" );
        Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Settings could not load." + newline + ex.ToString() );
        Debug.WriteLine( "[" + DateTime.Now.ToString() + "] " + "Settings could not load." + newline + ex.ToString() );
        Console.WriteLine( "Thread will sleep 10m!" );
        Thread.Sleep( 1000 * 60 * 10 ); //10m
        continue;
    }
    #endregion

    #region First Run
    if( first_run ) {
        first_run = false;
        if( db_helper.SetProductSyncWorking( customer_id, false ) && db_helper.SetOrderSyncWorking( customer_id, false ) && db_helper.SetNotificationSyncWorking( customer_id, false ) && db_helper.SetXmlSyncWorking( customer_id, false ) && db_helper.SetInvoiceSyncWorking( customer_id, false ) )
            Console.WriteLine( "[" + DateTime.Now.ToString() + "] " + customer_id + "-ID sync statuses reset for first run." );

        if( File.Exists( Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "local" ) ) ) {
            Helper.global.netsis.rest_url = "http://88.247.120.127:7070/";
            Helper.global.entegra.api_url = "http://88.247.120.127:5421/";
        }
    }
    #endregion

    #region Decision to Work
    if( customer.product_sync_status && !customer.is_productsync_working ) {
        if( customer.last_product_sync_date != null )
            if( customer.last_product_sync_date > DateTime.Now.AddSeconds( customer.product_sync_timer * -1 ) )
                customer.product_sync_status = false;
    }
    if( customer.order_sync_status && !customer.is_ordersync_working ) {
        if( customer.last_order_sync_date != null )
            if( customer.last_order_sync_date > DateTime.Now.AddSeconds( customer.order_sync_timer * -1 ) )
                customer.order_sync_status = false;
    }
    if( customer.xml_sync_status && !customer.is_xmlsync_working ) {
        if( customer.last_xml_sync_date != null )
            if( customer.last_xml_sync_date > DateTime.Now.AddSeconds( customer.xml_sync_timer * -1 ) )
                customer.xml_sync_status = false;
    }
    if( customer.invoice_sync_status && !customer.is_invoicesync_working ) {
        if( customer.last_invoice_sync_date != null )
            if( customer.last_invoice_sync_date > DateTime.Now.AddSeconds( customer.invoice_sync_timer * -1 ) )
                customer.invoice_sync_status = false;
    }
    if( customer.notification_sync_status && !customer.is_notificationsync_working ) {
        if( customer.last_notification_sync_date != null )
            if( customer.last_notification_sync_date > DateTime.Now.AddSeconds( customer.notification_sync_timer * -1 ) )
                customer.notification_sync_status = false;
    }
    if( !customer.product_sync_status && !customer.order_sync_status && !customer.xml_sync_status && !customer.invoice_sync_status && !customer.notification_sync_status ) {
        //Console.WriteLine( "Thread will sleep 5secs!" );
        Thread.Sleep( 5 * 1000 ); //5secs
        continue;
    }
    #endregion

    try {
        MainLoop main_loop = new( thread_id, customer, db_helper );
        if( Helper.global != null ) {
            if( main_loop.DoWork() ) {
                if( !customer.is_productsync_working && !customer.is_ordersync_working && !customer.is_xmlsync_working && !customer.is_invoicesync_working && !customer.notification_sync_status ) {
                    WriteLogLine( "[" + DateTime.Now.ToString() + "] " + customer_id.ToString() + "-" +
                    Helper.global.settings.company_name + " SYNC COMPLETED", ConsoleColor.Blue );
                }
            }
            else {
                WriteLogLine( "[" + DateTime.Now.ToString() + "] " + customer_id.ToString() + "-" +
                    Helper.global.settings.company_name + " SYNC ERROR", ConsoleColor.Red );
            }
        }
    }
    catch( Exception _ex ) {
        db_helper.LogToServer( thread_id, "error", _ex.Message + newline + _ex.ToString(), customer_id, "general" );
        //GMail.Send( Constants.mail_sender, Constants.mail_password, Constants.mail_sender_name, Constants.mail_to,
        //    Assembly.GetCallingAssembly().GetName().Name + "-Error  //customer:" + customer_id.ToString(),
        //    "ERROR" + newline + "CustomerID: " + customer_id.ToString() + ". " + _ex.Message + newline + _ex.ToString() + newline + "exit -2" );
        Console.WriteLine( _ex.Message + newline + _ex.ToString() );
        Console.WriteLine( "Thread will sleep 1m!" );
        Thread.Sleep( 1000 * 60 * 1 ); //1m
        continue;
    } finally {

    }
}

void CurrentDomain_ProcessExit( object? sender, EventArgs e ) {
    if( db_helper.LogToServer( thread_id, "merchanter", "ended " + thread_id, customer_id, "main_thread" ) ) {
        Debug.WriteLine( "exit success" );
    }
}

void WriteLogLine( string value, ConsoleColor _color ) {
    Console.ForegroundColor = _color;
    Console.WriteLine( value.PadRight( Console.WindowWidth - 1 ) );
    Console.ResetColor();
}