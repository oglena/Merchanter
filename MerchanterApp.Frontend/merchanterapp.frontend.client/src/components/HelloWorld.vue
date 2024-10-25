<template>
    <div class="weather-component">
        <h1>Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>

        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="post" class="content">
            <table>
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="products in post" :key="products.sku">
                        <td>{{ products.sku }}</td>
                        <td>{{ products.total_qty }}</td>
                        <td>{{ products.barcode }}</td>
                        <td>{{ products.price }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import axios from 'axios';

    export default defineComponent({
        data() {
            return {
                loading: false,
                post: null
            };
        },
        async created() {
            // fetch the data when the view is created and the data is
            // already being observed
            await this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            async fetchData() {
                this.post = null;
                this.loading = true;
                
                var response = await axios.get('http://localhost:5555/api/Product/GetProducts',{
                    headers: {
                        Authorization: "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyTmFtZSI6InFwYmlsaXNpbSIsImN1c3RvbWVySWQiOiIxIiwibmJmIjoxNzI5ODc3NjQ1LCJleHAiOjE3Mjk5MDc2NDUsImlzcyI6IkNhbiDDlnprYXltYWsiLCJhdWQiOiJDZXJlcyJ9.uCmVFw6pR5m9GjQzPa2e0UfQSnxGxIbD5DU1INl_ijI"
                    }
                });
                console.log(response);
                this.post = response.data.data;
                this.loading = false;
            }
        },
    });
</script>

<style scoped>
    th {
        font-weight: bold;
    }

    th, td {
        padding-left: .5rem;
        padding-right: .5rem;
    }

    .weather-component {
        text-align: center;
    }

    table {
        margin-left: auto;
        margin-right: auto;
    }
</style>
