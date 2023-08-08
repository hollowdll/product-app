<script lang="ts">
    import { PRODUCT_API_URL } from "../utils/const";
    import type { Product } from "../types/product";
	import { onMount } from "svelte";

    let products: Array<Product> = [];

    onMount(async () => {
        fetch(`${PRODUCT_API_URL}/products`)
            .then(response => {
                if (!response.ok) {
                    throw new Error("Failed to fetch products: " + response.statusText);
                }

                return response.json();
            })
            .then(data => products = data)
            .catch(err => console.error(err));
    });
</script>

<div class="products">
    {#if products.length === 0}
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
                        <td>{product.published}</td>
                    </tr>
                {/each}
            </tbody>
        </table>
    {/if}
</div>
