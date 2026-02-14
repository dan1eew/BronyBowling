<template>
  <div class="layout">
    <AppHeader />
    <main class="main-content">
      <router-view />
    </main>
  </div>
</template>

<script setup>
  import AppHeader from './components/AppHeader.vue'
</script>

<style>
  * {
    box-sizing: border-box;
  }

  body {
    margin: 0;
    font-family: 'Inter', system-ui, sans-serif;
    background: linear-gradient(135deg, #0a0f1c, #1a2338);
    color: white;
    min-height: 100vh;
  }

  .layout {
    min-height: 100vh;
    display: flex;
    flex-direction: column;
  }

  .main-content {
    flex: 1;
    padding-top: 80px;
  }
</style>
