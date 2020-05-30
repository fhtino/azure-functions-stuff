<template>
    <div class="cartvue">
        <div style="font-size:x-large; font-weight:bold;">
            My cart
        </div>
        <div v-for="item in cartitems">
            <div class="cartvueitem">
                {{item.id}} - {{item.description}} : &euro; {{item.price.toFixed(2)}}
            </div>
        </div>
        <br />
        <table width="100%">
            <tr>
                <td>
                    Total items: {{itemscount}}<br />
                    Total cost:  &euro; {{totalCost.toFixed(2)}}
                </td>
                <td align="right">
                    <div style="text-align:right;">
                        <button @click="confirmCart" v-if="itemscount>0">CONFIRM</button>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</template>

<script>
    module.exports = {
        data: function () {
            return {
                clickcount: 0
            }
        },
        props: {
            cartitems: { type: Array }
        },
        methods: {
            confirmCart: function () {
                this.$emit('confirm-cart', this.cartitems);
            }
        },
        computed: {
            itemscount() {
                if (this.cartitems)
                    return (this.cartitems.length);
                else
                    return 0;
            },
            totalCost() {
                var total = 0;
                if (this.cartitems && this.cartitems.length > 0) {
                    this.cartitems.forEach(function (item) {
                        total += item.price;
                    });
                }
                return total;
            }
        }
    }
</script>

<style>
    .cartvue {
        background-color: #cef6ff;
        padding: 5px;
        margin: 10px;
        border: 2px dashed;
    }

    .cartvueitem {
        margin-bottom: 2px;
        margin-top: 10px;
        padding: 2px;
        background-color: #0592b2;
        color: white;
    }
</style>