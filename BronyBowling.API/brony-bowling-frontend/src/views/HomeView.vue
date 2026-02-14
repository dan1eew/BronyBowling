<template>
  <div class="home">
    <div class="hero">
      <div class="hero-content">
        <h1>🎳 <span class="neon">Brony Bowling</span></h1>
        <p class="subtitle">Лучший боулинг в городе.<br>Бронируй онлайн — играй без очереди.</p>
        <router-link to="/booking" class="big-cta">
          Забронировать дорожку
        </router-link>
      </div>
    </div>

    <div class="features">
      <div class="feature" v-for="(f,i) in features" :key="i">
        <div class="icon">{{ f.icon }}</div>
        <h3>{{ f.title }}</h3>
        <p>{{ f.desc }}</p>
      </div>
    </div>
  </div>
</template>

<script setup>
  const features = [
    { icon: '⚡', title: 'Моментальное бронирование', desc: 'За 10 секунд' },
    { icon: '🎯', title: 'До 12 дорожек', desc: 'Для компаний и турниров' },
    { icon: '🏆', title: 'Без регистрации', desc: 'Или с личным кабинетом' }
  ]
</script>

<style scoped>
  .hero {
    height: 92vh;
    background: linear-gradient(rgba(10,15,28,0.85), rgba(10,15,28,0.95)), url('https://images.unsplash.com/photo-1600585154340-be6161a56a9c?q=80&w=2070') center/cover;
    display: flex;
    align-items: center;
    justify-content: center;
    text-align: center;
  }

  .neon {
    background: linear-gradient(90deg, #22c55e, #c026d3);
    -webkit-background-clip: text;
    background-clip: text;
    color: transparent;
  }

  .big-cta {
    display: inline-block;
    margin-top: 40px;
    padding: 20px 56px;
    font-size: 1.4rem;
    font-weight: 700;
    background: linear-gradient(90deg, #22c55e, #10b981);
    border-radius: 9999px;
    box-shadow: 0 20px 40px rgba(34, 197, 94, 0.4);
    transition: all 0.4s;
  }

    .big-cta:hover {
      transform: scale(1.08) translateY(-6px);
      box-shadow: 0 30px 60px rgba(34, 197, 94, 0.6);
    }

  .features {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
    gap: 40px;
    padding: 80px 40px;
    max-width: 1200px;
    margin: 0 auto;
  }
</style>
