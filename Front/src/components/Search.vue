<template>
  <div class="is-flex is-flex-wrap-nowrap is-justify-content-space-around mt-5">
    <div
      class="
        card
        search-interlocutor
        is-flex is-flex-wrap-wrap is-justify-content-space-around
        pt-2
        pb-2
      "
    >
      <b-field class="search-interlocutor__input">
        <b-input
          placeholder="Найти друга по ФИО"
          v-model="selected"
          icon="search"
          icon-pack="fas"
          :icon-right="selected ? 'times-circle' : ''"
          icon-right-clickable
          @icon-right-click="clearSearch"
          trap-focus
        >
        </b-input>
      </b-field>
      <b-button type="is-danger">Найти</b-button>
      <div
        class="
          search-interlocutor__group
          is-flex is-flex-wrap-wrap is-justify-content-center
        "
      >
        <div
          v-for="interlocutor in searchionInterlocutors"
          v-bind:key="interlocutor.id"
          class="
            person__group
            is-flex is-flex-wrap-nowrap is-justify-content-start
          "
        >
          <div class="is-flex is-flex-wrap-nowrap is-justify-content-center">
            <figure class="image is-64x64 mr-3">
              <img src="@/assets/person.png" />
            </figure>
            <p
              class="
                person__name person__name-size
                is-flex is-flex is-align-self-center
              "
            >
              {{ interlocutor.surname }} {{ interlocutor.name }}
              {{ interlocutor.patronymic }}
            </p>
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
        is-flex is-flex-wrap-wrap is-justify-content-center
        pt-1
        pb-2
      "
    >
      <p class="subtitle is-5">Рекомендуемые друзья</p>
      <div
        class="
          recommendation-interlocutor__group
          is-flex is-flex-wrap-wrap is-justify-content-center
        "
      >
        <div
          v-for="interlocutor in recommendationInterlocutors"
          v-bind:key="interlocutor.id"
          class="
            person__group
            is-flex is-flex-wrap-nowrap is-justify-content-space-center
            pb-5
          "
        >
          <figure class="image is-128x128 is-flex is-align-self-center">
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

export default Vue.extend({
  data() {
    return {
      filteredTags: undefined,
      recommendationInterlocutors: [
        {
          id: 0,
          surname: "Иванов",
          name: "Иван",
          patronymic: "Иванович",
          img: "@/assets/person.png",
          interests: [
            {
              id: 0,
              title: "Книги",
            },
            {
              id: 1,
              title: "Музыка",
            },
          ],
        },
        {
          id: 1,
          surname: "Петров",
          name: "Петр",
          patronymic: "Петрович",
          img: "@/assets/person.png",
          interests: [
            {
              id: 0,
              title: "Кинофильмы",
            },
            {
              id: 1,
              title: "Музыка",
            },
          ],
        },
      ],
      searchionInterlocutors: [
        {
          id: 0,
          surname: "Викторов",
          name: "Виктор",
          patronymic: "Викторович",
          img: "@/assets/person.png",
        },
        {
          id: 1,
          surname: "Александров",
          name: "Алексей",
          patronymic: "Александрович",
          img: "@/assets/person.png",
        },
        {
          id: 2,
          surname: "Иванов",
          name: "Иван",
          patronymic: "Иванович",
          img: "@/assets/person.png",
        },
      ],
    };
  },
  created() {},
  watch: {
    // call again the method if the route changes
    $route: "fetchData",
  },
  methods: {},
});
</script>

<style>
.search-interlocutor {
  width: 40%;
}

.recommendation-interlocutor {
  width: 50%;
}

.search-interlocutor__input {
  width: 70%;
}

.person__group {
  width: 90%;
}

.search-interlocutor__group,
.recommendation-interlocutor__group {
  width: 100%;
}

.person__name-size {
  width: 240px;
}
</style>
