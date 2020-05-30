<template>
    <div class="ordervue">
        <div style="font-size:x-large; font-weight:bold; margin-bottom:5px;">
            My order
        </div>
        <table>
            <tr><td>ID:</td><td>{{order.orderID}}</td></tr>
            <tr><td>Total:</td><td>&euro; {{order.totalAmount.toFixed(2)}}</td></tr>
            <tr><td>Order Date:</td><td>{{dtFormat}}</td></tr>
            <tr><td>Delivery Date:</td><td>{{deliveryDateFormat}}</td></tr>
        </table>
        <div v-for="item in order.items">
            <div class="ordervueitem">
                {{item.id}} - {{item.description}} : &euro; {{item.price.toFixed(2)}}
            </div>
        </div>

    </div>
</template>

<script>
    module.exports = {
        props: {
            order: { type: Object }
        },
        computed: {
            dtFormat() {
                return this.aaaa(this.order.dt);
            },
            deliveryDateFormat() {
                return this.aaaa(this.order.deliveryDate);
            }
        },
        methods: {
            aaaa: function (dateStr) {
                var date = new Date(dateStr);
                var options = { year: 'numeric', month: 'numeric', day: 'numeric', hour: 'numeric', minute: 'numeric' };
                return date.toLocaleDateString("it-IT", options);
            }
        },
    }
</script>

<style>
    .ordervue {
        background-color: #ceffce;
        padding: 5px;
        margin: 10px;
        border: 2px dashed;
    }

    .ordervueitem {
        margin-bottom: 2px;
        margin-top: 10px;
        padding: 2px;
        background-color: #049b04;
        color: white;
    }
</style>