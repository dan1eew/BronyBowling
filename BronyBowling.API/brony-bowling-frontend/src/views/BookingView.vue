<template>
  <div class="booking">
    <div class="container">
      <h1>Бронирование</h1>

      <div class="panel">
        <BaseInput v-model="date" type="date" placeholder="Дата" />
        <BaseInput v-model="start" type="time" placeholder="Начало" />
        <BaseInput v-model="end" type="time" placeholder="Конец" />
        <button @click="search" class="search-btn">Найти</button>
      </div>

      <div v-if="lanes.length" class="lanes">
        <div v-for="lane in lanes" :key="lane.bowlingLaneId"
             class="lane" :class="{ selected: laneId === lane.bowlingLaneId }"
             @click="laneId = lane.bowlingLaneId">
          №{{ lane.number }}
        </div>
      </div>

      <div v-if="!isAuth" class="guest">
        <BaseInput v-model="guestFullName" placeholder="ФИО" />
        <BaseInput v-model="guestPhone" type="tel" placeholder="Телефон" />
      </div>

      <button v-if="laneId" @click="book" class="book-btn">Забронировать</button>
    </div>
  </div>
</template>

<style scoped>
  .container {
    max-width: 920px;
    margin: 0 auto;
    padding: 80px 20px;
  }

  .panel {
    display: flex;
    gap: 12px;
    margin-bottom: 40px;
  }

  .search-btn, .book-btn {
    padding: 15px 32px;
    background: #22c55e;
    color: black;
    font-weight: 700;
    border-radius: 10px;
    white-space: nowrap;
  }

  .lanes {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(140px, 1fr));
    gap: 12px;
  }

  .lane {
    padding: 18px;
    background: #111827;
    border: 1px solid #334155;
    border-radius: 10px;
    text-align: center;
    cursor: pointer;
  }

    .lane.selected {
      border-color: #22c55e;
      background: rgba(34,197,94,0.1);
    }
</style>
