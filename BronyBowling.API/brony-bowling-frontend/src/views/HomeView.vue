<template>
  <div class="home">
    <header class="hero">
      <h1>🎳 Brony Bowling</h1>
      <p>Бронируйте дорожки онлайн быстро и удобно</p>
      <router-link to="/booking" class="cta">
        Забронировать дорожку
      </router-link>
    </header>

    <section class="features">
      <div class="card">
        <h3>⚡ Мгновенное бронирование</h3>
        <p>Выберите дату, время и дорожку за пару кликов</p>
      </div>

      <div class="card">
        <h3>📱 Для гостей</h3>
        <p>Можно бронировать без регистрации</p>
      </div>

      <div class="card">
        <h3>🔐 Личный кабинет</h3>
        <p>Управляйте своими бронями</p>
      </div>
    </section>
  </div>
</template>

<style scoped>
  .home {
    color: white;
    padding: 40px;
  }

  .hero {
    text-align: center;
    margin-bottom: 50px;
  }

    .hero h1 {
      font-size: 48px;
    }

  .cta {
    display: inline-block;
    margin-top: 20px;
    padding: 12px 24px;
    background: #42b883;
    border-radius: 8px;
    text-decoration: none;
    color: white;
  }

  .features {
    display: flex;
    gap: 20px;
    justify-content: center;
  }

  .card {
    background: rgba(255,255,255,0.1);
    padding: 20px;
    border-radius: 12px;
    width: 250px;
  }
</style>
