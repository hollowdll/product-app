<script lang="ts">
	import { onMount } from "svelte";

    onMount(async () => {
        logOut();
    })

    let logOut = () => {
		localStorage.removeItem("jwtAccessToken");
	}
</script>

<section>
    <h1>You have been signed out</h1>

    <a href="/">Return back to home page</a>
</section>
