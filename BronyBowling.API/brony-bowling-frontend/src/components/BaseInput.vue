<template>
  <div class="input-group">
    <label v-if="label">{{ label }}</label>
    <input :type="type"
           v-model="modelValue"
           :placeholder="placeholder"
           class="neon-input"
           @focus="focused = true"
           @blur="focused = false" />
  </div>
</template>

<script setup>
  import { defineProps, defineEmits, ref } from 'vue'
  const props = defineProps({
    modelValue: String,
    label: String,
    type: { default: 'text' },
    placeholder: String
  })
  const emit = defineEmits(['update:modelValue'])
  const focused = ref(false)
</script>

<style scoped>
  .input-group {
    margin-bottom: 24px;
  }

  label {
    display: block;
    margin-bottom: 8px;
    font-size: 13px;
    font-weight: 600;
    color: #a5b4fc;
    text-transform: uppercase;
    letter-spacing: 1px;
  }

  .neon-input {
    width: 100%;
    padding: 18px 20px;
    background: rgba(255,255,255,0.06);
    border: 2px solid rgba(255,255,255,0.12);
    border-radius: 16px;
    color: white;
    font-size: 17px;
    transition: all 0.4s ease;
  }

    .neon-input:focus {
      border-color: #22c55e;
      box-shadow: 0 0 25px rgba(34, 197, 94, 0.4);
      background: rgba(255,255,255,0.1);
    }
</style>
