<template>
  <div
    class="content-auth is-flex is-justify-content-center is-align-items-center"
  >
    <div class="card form is-flex is-justify-content-center is-flex-wrap-wrap p-4">
      <p class="subtitle is-5">Чтобы войти введите логин и пароль</p>
      <b-field label="Логин" class="form__input">
        <b-input v-model="login" required/>
      </b-field>
      <b-field label="Пароль" class="form__input">
        <b-input v-model="password" type="password" required password-reveal icon-pack="fas"/>
      </b-field>
      <div class="form__buttons is-flex is-justify-content-start">
        <b-button type="is-primary" @click="auth">Войти</b-button>
        <b-button type="is-danger is-light" @click="goReg">Регистрация</b-button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { ToastProgrammatic as Toast } from 'buefy';
import Data from "../utils/Data";

export default Vue.extend({
  data() {
    return {
      login: undefined,
      password: undefined
    };
  },
  created() {},
  methods: {
    goReg() {
      this.$router.push('/register');
    },

    auth() {
      Data.jsonQuery('account/auth', {
        login: this.login,
        password: this.password
      }).then(() => {
        this.$router.push('/');
      }).catch(() => {
        Toast.open({message: 'Неправильный логин или пароль', type: 'is-danger' })
      });
    }
  }
});
</script>

<style>
.form {
  width: 30%;
}

.form__input {
  width: 100%;
}

.form__buttons {
  width: 100%;
  gap: 10px;
}

.content-auth {
  height: 100vh;
}
</style>
