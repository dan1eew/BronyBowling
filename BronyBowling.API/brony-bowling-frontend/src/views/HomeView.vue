<template>
  <div class="home">
    <div class="content">
      <h1>Brony Bowling</h1>
      <p class="tagline">Премиальный боулинг-клуб</p>
      <div class="address">
        Мегаполис<br>
        г. Уфа ул. Менделеева 137
      </div>
      <router-link to="/booking" class="cta">Забронировать</router-link>
    </div>
  </div>
</template>

<style scoped>
  .home {
    min-height: 100vh;
    display: flex;
    align-items: center;
    padding-left: 120px;
    background: linear-gradient(rgba(0,0,0,0.85), rgba(0,0,0,0.9)), url('https://images.unsplash.com/photo-1600585154340-be6161a56a9c?q=80') center/cover;
  }

  .content {
    max-width: 460px;
  }

  h1 {
    font-size: 3.8rem;
    font-weight: 800;
    color: #22c55e;
    margin: 0 0 12px;
  }

  .tagline {
    font-size: 1.35rem;
    margin-bottom: 48px;
    opacity: 0.9;
  }

  .address {
    font-size: 1.25rem;
    line-height: 1.6;
    margin-bottom: 52px;
    opacity: 0.85;
  }

  .cta {
    padding: 15px 48px;
    background: #22c55e;
    color: black;
    font-size: 1.1rem;
    font-weight: 700;
    border-radius: 50px;
    text-decoration: none;
  }
</style>
