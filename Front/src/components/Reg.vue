<template>
  <div class="content-auth is-flex is-justify-content-center is-align-items-center">
    <div class="card form is-flex is-justify-content-center is-flex-wrap-wrap p-4">
       <p class="subtitle is-5">Для регистрации укажите логин и пароль</p>
      <b-field label="Логин" class="form__input">
        <b-input v-model="name"></b-input>
      </b-field>
      <b-field label="Пароль" class="form__input">
        <b-input 
        type="password" 
        password-reveal
        icon-pack="fas"
        > 
        </b-input>
      </b-field>
      <div class="form__buttons is-flex is-justify-content-start">
         <b-button type="is-primary">Войти</b-button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";

export default Vue.extend({
  data(): object {
    return {};
  },
  created() {},
  methods: {},
});
</script>

<style>
</style>
