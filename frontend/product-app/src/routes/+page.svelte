<script lang="ts">
	import { PRODUCT_API_URL } from "../utils/const";
	import type { UserData } from "../types/user";
	import { onMount } from "svelte";

	let userData: UserData | null = null;
	let loading = true;

	onMount(async () => {
		await fetchUserData();
		loading = false;
	})

	let fetchUserData = async () => {
		const jwtAccessToken = localStorage.getItem("jwtAccessToken");

        fetch(`${PRODUCT_API_URL}/account/currentuser`, {
            method: "GET",
            headers: { "Authorization": `Bearer ${jwtAccessToken ?? ""}` }
        })
        .then(response => {
            if (response.ok) {
				return response.json();
			}
        })
        .then(data => userData = data)
        .catch(err => console.error(err));
	}

	let logOut = () => {
		localStorage.removeItem("jwtAccessToken");
	}
</script>

<svelte:head>
	<title>Home</title>
	<meta name="description" content="Fullstack product app with JWT authentication" />
</svelte:head>

<section>
	<h1>Welcome to Product app</h1>

	{#if loading == false}
		{#if userData != null}
			<p>Logged in as user <strong>{userData.username}</strong></p>
		{:else}
			<p>You are not logged in</p>
		{/if}

		<a href="/login">Login</a>
		<a href="/register">Register</a>
	{/if}

</section>

<style>
	
</style>
