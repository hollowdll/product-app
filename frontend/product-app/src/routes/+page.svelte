<script lang="ts">
	import { PRODUCT_API_URL } from "../utils/const";
	import type { UserData } from "../types/user";
	import { onMount } from "svelte";

	let userData: UserData | null = null;
	let isLoggedIn = false;

	onMount(async () => {
		fetchUserData();
	})

	let fetchUserData = () => {
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
        .then(data => {
			userData = data
			isLoggedIn = true
		})
        .catch(err => console.error(err));
	}
</script>

<svelte:head>
	<title>Home</title>
	<meta name="description" content="Fullstack product app with JWT authentication" />
</svelte:head>

<section>
	<h1>Welcome to Product app</h1>

	{#if userData != null && isLoggedIn == true}
		<p>Logged in as user <strong>{userData.username}</strong></p>

		<a href="/logout">Sign out</a>
	{:else}
		<p>You are not logged in</p>
		<p>Log in with one of the test users or create a new one</p>
		<p>
			TestUser / Password1! <br>
			AdminUser / Password2!
		</p>

		<div>
			<a href="/login">Login</a>
			<a href="/register">Register</a>
		</div>
	{/if}

</section>

<style>
	
</style>
