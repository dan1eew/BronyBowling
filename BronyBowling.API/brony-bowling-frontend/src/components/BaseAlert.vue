<template>
  <input :type="type"
         :placeholder="placeholder"
         :value="modelValue"
         @input="$emit('update:modelValue', $event.target.value)"
         class="input" />
</template>

<script setup>
  import { defineProps, defineEmits } from 'vue'
  defineProps({ modelValue: String, type: { default: 'text' }, placeholder: String })
  defineEmits(['update:modelValue'])
</script>

<style scoped>
  .input {
    width: 100%;
    padding: 15px 20px;
    background: #111827;
    border: 1px solid #334155;
    border-radius: 10px;
    color: white;
    font-size: 15px;
  }

    .input:focus {
      border-color: #22c55e;
      outline: none;
    }
</style>
