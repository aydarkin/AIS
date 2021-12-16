<template>
  <div
    class="
      Search
      is-flex is-flex-wrap-nowrap is-justify-content-space-around
      p-5
      is-flex-grow-1
    "
  >
    <div
      class="
        card
        search-interlocutor
        is-flex is-flex-direction-column
        pt-2
        pb-2
      "
    >
      <div class="is-flex is-flex-wrap-wrap is-justify-content-center p-4">
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
          search-interlocutor__group
          is-flex-grow-1 is-flex is-flex-direction-column is-flex-shrink-1
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
          <b-button
            type="is-danger is-light is-flex is-align-self-center"
            @click="handleFriendship(interlocutor)"
          >
            {{
              interlocutor.type == 2
                ? "Удалить из друзей"
                : interlocutor.type == 1
                ? "Принять заявку"
                : interlocutor.type == 0
                ? "Отменить заявку"
                : "Отправить заявку"
            }}</b-button
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
          class="interlocutor__group is-flex is-flex-wrap-nowrap p-2"
        >
          <figure
            class="
              image
              is-128x128 is-flex is-align-self-center is-flex-shrink-0
            "
          >
            <img src="@/assets/person.png" />
          </figure>
          <div>
            <p class="person__name person__name-size is-align-self-center">
              {{ interlocutor.surname }} {{ interlocutor.name }}
              {{ interlocutor.patronymic }}
            </p>
            <p v-if="interlocutor.cityId">{{ interlocutor.city.title }}</p>
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
            <b-button type="is-danger is-light is-flex is-align-self-center" @click="addFriendship(interlocutor)"
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
import { ToastProgrammatic as Toast } from "buefy";

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

    this.find();
    this.loadRecommended();
  },
  watch: {
    // call again the method if the route changes
    $route: "fetchData",
  },
  methods: {
    async loadRecommended() {
      const promises = [];
      promises.push(
        Data.getQuery("person", {
          mode: "recommended",
          id: this.myId,
        })
      );
      const [recommended] = await Promise.all(promises);

      this.recommendationInterlocutors = recommended;
    },

    async find() {
      const promises = [];
      promises.push(Data.getQuery("person", { fio: this.selected || "" }));
      promises.push(Data.getQuery("friendship/" + this.myId));
      const [persons, friendships] = await Promise.all(promises);

      const friends = friendships
        .filter((f: any) => f.direction == 2)
        .map((f: any) => (f.firstId == this.myId ? f.second : f.first));

      const subscribers = friendships
        .filter(
          (f: any) =>
            (f.firstId == this.myId && f.direction == 1) ||
            (f.secondId == this.myId && f.direction == 0)
        )
        .map((f: any) => (f.firstId == this.myId ? f.second : f.first));

      const subscriptions = friendships
        .filter(
          (f: any) =>
            (f.firstId == this.myId && f.direction == 0) ||
            (f.secondId == this.myId && f.direction == 1)
        )
        .map((f: any) => (f.firstId == this.myId ? f.second : f.first));

      persons.forEach((person: any) => {
        person.type = null;

        if (friends.find((f: any) => f.userId == person.userId)) {
          person.type = 2;
        } else if (subscribers.find((f: any) => f.userId == person.userId)) {
          person.type = 1;
        } else if (subscriptions.find((f: any) => f.userId == person.userId)) {
          person.type = 0;
        }
      });

      this.searchInterlocutors = persons;
    },
    clearSearch() {
      this.selected = "";
    },

    addFriendship(person: any) {
      Data.jsonQuery("friendship", {
        firstId: this.myId,
        secondId: person.userId,
      })
        .then(() => {
          return Promise.all([this.find(), this.loadRecommended()])
        })
        .then(() => {
          Toast.open({
            message: "Операция прошла успешно",
            type: "is-success",
          });
        })
        .catch(() => {
          Toast.open({ message: "Операция не удалась", type: "is-danger" });
        });
    },

    deleteFriendship(person: any) {
      Data.deleteQuery(`friendship?from=${this.myId}&to=${person.userId}`)
        .then(() => {
          return Promise.all([this.find(), this.loadRecommended()])
        })
        .then(() => {
          Toast.open({
            message: "Операция прошла успешно",
            type: "is-success",
          });
        })
        .catch(() => {
          Toast.open({ message: "Операция не удалась", type: "is-danger" });
        });
    },

    handleFriendship(person: any) {
      if (person.type == 2 || person.type == 0) {
        this.deleteFriendship(person);
      } else {
        this.addFriendship(person);
      }
    },
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
