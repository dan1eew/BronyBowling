<template>
  <div class="alert" :class="type">
    <slot />
  </div>
</template>

<script setup>
  defineProps({ type: { default: 'info' } })
</script>

<style scoped>
  .alert {
    padding: 16px 20px;
    border-radius: 14px;
    font-weight: 500;
    margin-bottom: 24px;
    display: flex;
    align-items: center;
    gap: 12px;
  }

  .error {
    background: rgba(248, 113, 113, 0.15);
    color: #f87171;
    border: 1px solid #f87171;
  }

  .success {
    background: rgba(74, 222, 128, 0.15);
    color: #4ade80;
    border: 1px solid #4ade80;
  }
</style>
