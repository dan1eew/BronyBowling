<template>
  <div class="layout">
    <AppHeader />
    <router-view />
  </div>
</template>

<script setup>
  import AppHeader from './components/AppHeader.vue'
</script>

<style>
  body {
    margin: 0;
    font-family: Inter, sans-serif;
    background: linear-gradient(135deg, #0f172a, #1e293b);
  }
</style>
