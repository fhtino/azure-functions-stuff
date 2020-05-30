<template>
    <div class="proditemvue">
        <table>
            <tr>
                <td>
                    <div>
                        <img v-bind:src="imgsrc" />
                    </div>
                </td>
                <td>
                    <div>
                        <span style="font-size: smaller">ID: {{productobj.id}}</span>  <br />
                        <span style="font-size:larger;">{{productobj.description}}</span><br />
                        <span style="font-size: smaller">{{productobj.longDescription}}</span><br />
                        <table>
                        </table>
                        <div>
                            <div style="background-color:lightcyan; padding:5px; display:inline-block;">&euro; {{productobj.price.toFixed(2)}}</div>
                            <div style="display:inline-block; width:100px;"> </div>
                            <button @click="addToCart">BUY</button>
                        </div>
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
            productobj: Object
        },
        methods: {
            addToCart: function () {
                this.clickcount += 1;
                this.$emit('add-to-cart', this.productobj);
            }
        },
        computed: {
            imgsrc() { return 'prodimgs/' + this.productobj.id + '.png'; },            
        }
    }
</script>

<style>
    .proditemvue {
        background-color: lightgray;
        margin: 10px;
        padding: 5px;
        border: 1px solid;
        border-color: darkblue;
    }
</style>