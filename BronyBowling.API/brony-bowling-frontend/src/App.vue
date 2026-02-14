<template>
  <div class="app">
    <AppHeader />
    <main class="main">
      <router-view />
    </main>
  </div>
</template>

<script setup>
  import AppHeader from './components/AppHeader.vue'
</script>

<style>
  body {
    margin: 0;
    font-family: 'Inter', system-ui, sans-serif;
    background: #000000;
    color: #e0f2fe;
  }

  .app {
    min-height: 100vh;
    display: flex;
    flex-direction: column;
  }

  .main {
    flex: 1;
  }
</style>
