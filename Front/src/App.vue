<template>
  <div id="app">
    <Auth v-if="isAuth" />
    <Reg v-else-if="isReg" />
    <Page v-else :page="page" v-on:pageChange="pageChanged" />
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Page from "./components/Page.vue";
import Auth from "./components/Auth.vue";
import Reg from "./components/Reg.vue";
import Cookie from "./utils/Cookie";

type TPage = "profile" | "messenger" | "search" | undefined;

export default Vue.extend({
  name: "App",
  components: {
    Page,
    Auth,
    Reg,
  },
  data() {
    return {
      page: "profile" as TPage,
      isAuth: false,
      isReg: false,
    };
  },

  beforeCreate() {
    const page = window.location.pathname.replaceAll("/", "");
    if (!['auth', 'register'].includes(page)) {
      // проверяем наличие авторизации
      const id = Cookie.getCookie("userId");
      const token = Cookie.getCookie("token");
      if (!id || !token) {
        // нет авторизации
        this.$router.push('/auth');
      }
    }
  },

  beforeMount() {
    const page = window.location.pathname.replaceAll("/", "");
    this.isAuth = page === "auth";
    this.isReg = page === "register";

    if (["profile", "messenger", "search"].includes(page)) {
      this.page = page as TPage;
    }
  },
  watch: {
    $route() {
      const page = window.location.pathname.replaceAll("/", "");
      this.isAuth = page === "auth";
      this.isReg = page === "register";

      if (["profile", "messenger", "search"].includes(page)) {
        this.page = page as TPage;
      }
    },
  },
  methods: {
    pageChanged(page: TPage): void {
      this.page = page;
      this.$router.push({name: 'Messenger+', path: `/${page}`});
    },
  },
});
</script>

<style>
@import url("https://use.fontawesome.com/releases/v5.2.0/css/all.css");
html {
  overflow-y: auto !important;
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

::-webkit-scrollbar { width: 8px; }

::-webkit-scrollbar-track {
  background: #eeeeee;
  border-radius: 5px;
}

::-webkit-scrollbar-thumb {
  background: #bdbdbd;
  border-radius: 5px;
}
</style>
