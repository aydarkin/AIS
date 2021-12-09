<template>
  <div
    class="content-auth is-flex is-justify-content-center is-align-items-center"
  >
    <div
      class="card form is-flex is-justify-content-center is-flex-wrap-wrap p-4"
    >
      <p class="title is-5">Регистрация</p>
      <b-field label="Логин" class="form__input">
        <b-input v-model="login"></b-input>
      </b-field>
      <b-field label="Пароль" class="form__input">
        <b-input v-model="password" type="password" password-reveal icon-pack="fas"> </b-input>
      </b-field>
      <div class="form__buttons is-flex is-justify-content-start">
        <b-button type="is-danger" @click="reg">Сохранить</b-button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { ToastProgrammatic as Toast } from "buefy";
import Data from "../utils/Data";

export default Vue.extend({
  data() {
    return {
      login: undefined,
      password: undefined,
    };
  },
  created() {},
  methods: {
    reg() {
      Data.jsonQuery("account/register", {
        login: this.login,
        password: this.password,
      })
        .then(() => {
          this.$router.push("/auth");
        })
        .catch(() => {
          Toast.open({
            message: "Логин уже занят",
            type: "is-danger",
          });
        });
    }
  },
});
</script>

<style></style>
