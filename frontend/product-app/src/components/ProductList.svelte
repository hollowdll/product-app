<script lang="ts">
    import { PRODUCT_API_URL } from "../utils/const";
    import type { Product } from "../types/product";
	import { onMount } from "svelte";

    let products: Array<Product> = [];
    let errMessage = "";

    onMount(async () => {
        fetchProducts();
    });

    const fetchProducts = () => {
        const jwtAccessToken = localStorage.getItem("jwtAccessToken");

        fetch(`${PRODUCT_API_URL}/products`, {
            method: "GET",
            headers: { "Authorization": `Bearer ${jwtAccessToken ?? ""}` }
        })
        .then(response => {
            if (!response.ok) {
                if (response.status == 401) {
                    errMessage = "Log in to view products";
                } else {
                    errMessage = "Failed to load products";
                }
                throw new Error("Failed to fetch products");
            }

            return response.json();
        })
        .then(data => products = data)
        .catch(err => console.error(err));
    }
</script>

<div class="products">
    {#if errMessage.length > 0}
        <p>{errMessage}</p>
    {:else if products.length === 0}
        <p>Loading...</p>
    {:else}
        <table>
            <thead>
                <tr>
                    <th>Product ID</th>
                    <th>Name</th>
                    <th>Manufacturer</th>
                    <th>Price</th>
                    <th>Published</th>
                </tr>
            </thead>
            <tbody>
                {#each products as product}
                    <tr>
                        <td>{product.id}</td>
                        <td>{product.name}</td>
                        <td>{product.manufacturer}</td>
                        <td>{product.price}</td>
                        <td>{new Date(Date.parse(product.published)).toISOString()}</td>
                    </tr>
                {/each}
            </tbody>
        </table>
    {/if}
</div>
