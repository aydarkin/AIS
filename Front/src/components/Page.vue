<template>
  <div class="Page is-flex-grow-1 is-flex is-flex-direction-column">
    <b-navbar type="is-primary" :spaced="true" class="is-flex-shrink-0">
      <template #start>
        <b-navbar-item v-on:click="route('profile')">
          <b-icon pack="fas" icon="user" size="is-small" />
          <span class="ml-2"> Моя анкета</span>
        </b-navbar-item>
        <b-navbar-item v-on:click="route('messenger')">
          <b-icon pack="fas" icon="comments" size="is-small" />
          <span class="ml-2"> Сообщения</span>
        </b-navbar-item>
        <b-navbar-item v-on:click="route('search')">
          <b-icon pack="fas" icon="search" size="is-small" />
            <span class="ml-2">Найти</span>
        </b-navbar-item>
      </template>
      <template #end>
        <b-navbar-item tag="div">
          <div class="buttons">
            <a class="button is-danger" @click="exit">
              <strong>Выйти</strong>
            </a>
          </div>
        </b-navbar-item>
      </template>
    </b-navbar>
    <div class="Page__content is-flex is-flex-direction-column is-flex-grow-1">
      <Profile v-if="page === 'profile'" />
      <Messenger v-if="page === 'messenger'" />
      <Search v-if="page === 'search'" />
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Profile from "./Profile.vue";
import Messenger from "./Messenger.vue";
import Search from "./Search.vue";

type TPage = "profile" | "messenger" | "search";

export default Vue.extend({
  name: "Page",
  components: {
    Profile,
    Messenger,
    Search,
  },
  props: {
    page: String,
  },
  data() {
    return {};
  },
  created() {},
  watch: {},
  methods: {
    route(page: TPage): void {
      this.$emit("pageChange", page);
    },

    exit() {
      document.cookie = 'token=;userId=';
      this.$router.push('/auth');
    }
  },
});
</script>

<style>
.Page {
  max-height: 100vh;
  min-height: 100vh;
  max-width: 100vw;
  overflow: hidden;
}

.Page__content {
  overflow: hidden;
  width: 100%;
}
</style>
