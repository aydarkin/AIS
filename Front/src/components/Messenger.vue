<template>
  <div class="is-flex is-flex-wrap-wrap is-justify-content-space-around mt-5">
    <div class="card messenger mb-5">
      <b-tabs
        v-model="tabId"
        vertical
        :animated="false"
        @input="onTabChange"
        type="is-boxed"
      >
        <b-tab-item
          v-for="interlocutor in allInterlocutors"
          v-bind:key="interlocutor.id"
        >
          <template v-slot:header>
            <div
              class="
                person__item
                is-flex is-flex-wrap-nowrap is-justify-content-start
              "
            >
              <figure class="image is-32x32">
                <img src="@/assets/person.png" />
              </figure>
              <p class="is-flex is-align-self-center">
                {{ interlocutor.surname }} {{ interlocutor.name }}
                {{ interlocutor.patronymic }}
              </p>
            </div>
          </template>
          <div
            class="
              interlocutor
              is-flex is-align-items-start is-flex-direction-column-reverse
            "
          >
            <div
              v-for="message in currentMessages"
              v-bind:key="message.id"
              class="interlocutor__message is-size-6 is-flex"
            >
              <div
                class="
                  interlocutor__avatar
                  is-flex is-justify-content-center is-flex-wrap-wrap
                "
              >
                <figure class="image is-32x32">
                  <img src="@/assets/person.png" />
                </figure>
                <p class="person__name is-size-7">
                  {{
                    interlocutor.id == message.fromId ? interlocutor.name : "Вы"
                  }}
                </p>
              </div>
              <div class="is-flex is-flex-direction-column is-align-items-center">
                <p class="is-size-7">{{ getDateText(message.date)}}</p>
                <b-message
                  :type="interlocutor.id == message.fromId ? 'is-danger' : ''"
                >
                  {{ message.text }}
                </b-message>
              </div>
            </div>
          </div>
        </b-tab-item>
      </b-tabs>
    </div>
    <div
      class="
        card
        contact__form
        is-flex is-justify-content-space-around
        pt-2
        pb-2
      "
    >
      <b-field class="contact__form_textarea">
        <b-input placeholder="Напишите сообщение"></b-input>
      </b-field>
      <b-button type="is-primary">Отправить</b-button>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";

export default Vue.extend({
  data() {
    return {
      allInterlocutors: [
        {
          id: 2,
          surname: "Иванов",
          name: "Иван",
          patronymic: "Иванович",
          img: "@/assets/person.png",
          interests: [
            {
              id: 0,
              title: "Интерес 1",
            },
            {
              id: 1,
              title: "Интерес 2",
            },
          ],
        },
        {
          id: 3,
          surname: "Петров",
          name: "Петр",
          patronymic: "Петрович",
          img: "@/assets/person.png",
          interests: [
            {
              id: 0,
              title: "Интерес 1",
            },
            {
              id: 1,
              title: "Интерес 2",
            },
          ],
        },
      ],

      interlocutorId: 2,
      myId: 1,
      tabId: 1,
      messages: [
        {
          id: 1,
          fromId: 2,
          toId: 1,
          text: "Привет demo",
          image: null,
          date: "0001-01-01T00:00:00",
        },
        {
          id: 2,
          fromId: 1,
          toId: 3,
          text: "Привет demo2",
          image: null,
          date: "0001-01-01T00:00:00",
        },
        {
          id: 3,
          fromId: 1,
          toId: 2,
          text: "Привет demo1",
          image: null,
          date: "0001-01-01T00:00:00",
        },
      ],
    };
  },
  computed: {
    currentMessages(): object[] {
      // фильтр собеседник входящий и я исходящий или собеседник исходящий и я входящий

      return this.messages.filter((mes) => {
        return (
          (mes.fromId === this.interlocutorId && mes.toId === this.myId) ||
          (mes.fromId === this.myId && mes.toId === this.interlocutorId)
        );
      });
    },
  },
  created() {
    // грузим все сообщения в this.messages
  },
  watch: {
    // call again the method if the route changes
    $route: "fetchData",
  },
  methods: {
    onTabChange() {
      this.interlocutorId = this.allInterlocutors[this.tabId].id;
    },

    getDateText(dateISO: string): string {
      const date = new Date(dateISO);
      return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`
    }
  },
});
</script>

<style>
.person__item {
  gap: 8px;
  width: 200px;
}

.interlocutor__avatar {
  width: 32px;
}

.interlocutor__message,
.interlocutor {
  gap: 16px;
}

.interlocutor {
  overflow-y: auto;
  height: 480px;
}

.contact__form,
.messenger {
  width: 90%;
}

.contact__form_textarea {
  width: 80%;
}
</style>
