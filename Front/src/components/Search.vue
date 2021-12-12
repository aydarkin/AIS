<template>
  <div class="Search is-flex is-flex-wrap-nowrap is-justify-content-space-around p-5 is-flex-grow-1">
    <div
      class="
        card
        search-interlocutor
        is-flex is-flex-direction-column
        pt-2
        pb-2
      "
    >
      <div
        class="
          is-flex is-flex-wrap-wrap is-justify-content-center
          p-4
        "
      >
        <b-input
          placeholder="Найти друга по ФИО"
          v-model="selected"
          icon="search"
          icon-pack="fas"
          :icon-right="selected ? 'times-circle' : ''"
          icon-right-clickable
          @icon-right-click="clearSearch"
          trap-focus
          class="search-interlocutor__input mr-4 is-flex-grow-1"
        >
        </b-input>
        <b-button @click="find" type="is-danger">Найти</b-button>
      </div>
      <div
        class="
          search-interlocutor__group is-flex-grow-1
          is-flex is-flex-direction-column is-flex-shrink-1
        "
      >
        <div
          v-for="interlocutor in searchInterlocutors"
          v-bind:key="interlocutor.id"
          class="
            interlocutor__group
            is-flex is-flex-wrap-nowrap is-justify-content-start
            p-2
          "
        >
          <div
            class="
              person__info
              is-flex is-flex-wrap-nowrap is-justify-content-start
            "
          >
            <figure class="image is-64x64 mr-3 is-flex is-align-self-center">
              <img src="@/assets/person.png" />
            </figure>
            <div
              class="
                is-flex is-flex is-align-self-center is-flex is-flex-wrap-nowrap
              "
            >
              <div class="person__name person__name-size">
                {{ interlocutor.surname }} {{ interlocutor.name }}
                {{ interlocutor.patronymic }}
              </div>
            </div>
          </div>
          <b-button type="is-danger is-light is-flex is-align-self-center"
            >Отправить заявку</b-button
          >
        </div>
      </div>
    </div>
    <div
      class="
        card
        recommendation-interlocutor
        is-flex is-flex-direction-column
        pt-1
        pb-2
      "
    >
      <p class="subtitle is-5 has-text-centered">Рекомендуемые друзья</p>
      <div
        class="
          recommendation-interlocutor__group
          is-flex is-flex-direction-column is-flex-shrink-1
        "
      >
        <div
          v-for="interlocutor in recommendationInterlocutors"
          v-bind:key="interlocutor.id"
          class="
            interlocutor__group
            is-flex is-flex-wrap-nowrap
            p-2
          "
        >
          <figure class="image is-128x128 is-flex is-align-self-center is-flex-shrink-0">
            <img src="@/assets/person.png" />
          </figure>
          <div>
            <p class="person__name person__name-size is-align-self-center">
              {{ interlocutor.surname }} {{ interlocutor.name }}
              {{ interlocutor.patronymic }}
            </p>
            <b-field label="Интересы">
              <b-taginput
                :value="interlocutor.interests"
                ellipsis
                field="title"
                :closable="false"
                :readonly="true"
              >
              </b-taginput>
            </b-field>
            <p v-if="interlocutor.cityId">{{interlocutor.city.title}}</p>
            <b-button type="is-danger is-light is-flex is-align-self-center"
              >Отправить заявку</b-button
            >
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Data from "../utils/Data";
import Cookie from "../utils/Cookie";

export default Vue.extend({
  data() {
    return {
      filteredTags: undefined,
      myId: undefined as any,
      selected: "",
      recommendationInterlocutors: [],
      searchInterlocutors: [],
    };
  },
  async created() {
    this.myId = Cookie.getCookie("userId");

    const promises = [];
    promises.push(Data.getQuery("person", {
      mode: "recommended",
      id: this.myId
    }));

    this.find();
    const [recommended] = await Promise.all(
      promises
    );

    this.recommendationInterlocutors = recommended;
  },
  watch: {
    // call again the method if the route changes
    $route: "fetchData",
  },
  methods: {
    find() {
      Data.getQuery("person", { fio: this.selected || '' }).then((persons) => {
        this.searchInterlocutors = persons;
      });
    },
    clearSearch() {
      this.selected = '';
    }
  },
});
</script>

<style>
.Search {
  overflow: hidden;
}

.search-interlocutor {
  width: 40%;
  gap: 10px;

  overflow: hidden;
}

.recommendation-interlocutor {
  width: 50%;
  overflow: hidden;
}

.search-interlocutor__input {
}

.interlocutor__group {
  width: 90%;
  gap: 10px;
}

.search-interlocutor__group,
.recommendation-interlocutor__group {
  width: 100%;
  overflow: auto;
}

.person__info {
  width: 100%;
}

.person__name-size {
  width: 18vw;
}
</style>
