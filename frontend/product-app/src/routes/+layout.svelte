<script>
	import Header from './Header.svelte';
	import './styles.css';
</script>

<div class="app">
	<Header />

	<main>
		<slot />
	</main>

	<footer>
		<p>Source code for this app can be found <a href="https://github.com/hollowdll/product-app">here</a></p>
	</footer>
</div>

<style>
	
</style>
