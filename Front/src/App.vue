<template>
  <div id="app">
    <Auth v-if="isAuth" />
    <Reg v-if="isReg" />
    <Page v-else :page="page" v-on:pageChange="pageChanged" />
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Page from "./components/Page.vue";
import Auth from "./components/Auth.vue";
import Reg from "./components/Reg.vue";

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
      page: undefined as TPage,
      isAuth: false,
      isReg: false,
    };
  },
  beforeMount() {
    const page = window.location.pathname.replaceAll("/", "");
    console.log("before", page);
    if (["profile", "messenger", "search"].includes(page)) {
      this.page = page as TPage;
      this.isAuth = false;
      this.isReg = false;
    } else if (["auth"].includes(page)) {
      this.isAuth = true;
      this.isReg = false;
    } else if (["register"].includes(page)) {
      this.isAuth = false;
      this.isReg = true;
    }
  },
  watch: {
    $route() {
      const page = window.location.pathname.replaceAll("/", "");
      console.log("watch", page);
      if (["profile", "messenger", "search"].includes(page)) {
        this.page = page as TPage;
        this.isAuth = false;
      } else if (["auth", "register"].includes(page)) {
        this.isAuth = true;
      }
    },
  },
  methods: {
    pageChanged(page: TPage): void {
      this.page = page;
      window.history.pushState(null, "Messenger+", `/${page}`);
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

  max-height: 100vh;
  min-height: 100vh;
  max-width: 100vw;
  overflow: hidden;
}
</style>
