<template>
  <router-view />
</template>

<style>
  body {
    font-family: Arial;
    background: #0f172a;
    color: white;
    display: flex;
    justify-content: center;
    padding-top: 40px;
  }

  .card {
    background: #1e293b;
    padding: 30px;
    border-radius: 12px;
    width: 350px;
  }

  input, select, button {
    width: 100%;
    margin-top: 10px;
    padding: 10px;
  }

  button {
    background: #7c3aed;
    color: white;
    border: none;
    cursor: pointer;
  }
</style>
