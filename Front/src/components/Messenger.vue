<template>
  <div class="is-flex is-flex-wrap-wrap is-justify-content-space-around mt-5">
    <div class="card messenger mb-5" v-if="loaded">
      <b-tabs
        v-model="tabId"
        vertical
        :animated="false"
        @input="onTabChange"
        type="is-boxed"
      >
        <b-tab-item
          v-for="interlocutor in allInterlocutors"
          v-bind:key="interlocutor.userId"
        >
          <template v-slot:header>
            <div
              class="
                person__item
                is-clipped
                is-flex
                is-flex-wrap-nowrap
                is-justify-content-start
                is-align-items-center
              "
            >
              <figure class="image is-32x32 is-flex-shrink-0">
                <img src="@/assets/person.png" />
              </figure>
              <p
                class="person__name is-flex-shrink-1 is-flex-grow-1 is-clipped"
                :title="`${interlocutor.surname} ${interlocutor.name} ${interlocutor.patronymic}`"
              >
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
              class="
                interlocutor__message
                is-flex-shrink-0 is-size-6 is-flex is-clipped is-align-items-end
              "
            >
              <div
                class="interlocutor__avatar is-flex is-flex-direction-column is-align-items-center"
              >
                <figure class="image is-32x32">
                  <img src="@/assets/person.png" />
                </figure>
                <p
                  class="person__name is-size-7 width100 has-text-centered"
                  :title="
                    interlocutor.userId == message.fromId
                      ? interlocutor.name
                      : 'Вы'
                  "
                >
                  {{
                    interlocutor.userId == message.fromId
                      ? interlocutor.name
                      : "Вы"
                  }}
                </p>
              </div>
              <div class="is-flex is-flex-direction-column">
                <p class="is-size-7">{{ getDateText(message.date) }}</p>
                <b-message
                  class="interlocutor__text"
                  :type="
                    interlocutor.userId == message.fromId ? 'is-danger' : ''
                  "
                >
                  {{ message.text }}
                </b-message>
              </div>
            </div>
          </div>
        </b-tab-item>
      </b-tabs>
    </div>
    <div class="card contact__form is-flex pt-4 pl-4 pr-4 pb-2">
      <b-field class="contact__form_textarea is-flex-grow-1 mr-3">
        <b-input placeholder="Напишите сообщение"></b-input>
      </b-field>
      <b-button type="is-primary">Отправить</b-button>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Profile from "../utils/Profile";
import Data from "../utils/Data";
import Cookie from "@/utils/Cookie";

interface IMessage {
  fromId: number;
  toId: number;
  text: string;
}

export default Vue.extend({
  data() {
    return {
      loaded: false,

      profile: undefined,
      allInterlocutors: [] as any[],

      interlocutorId: undefined,
      myId: undefined as any,
      tabId: 0,
      messages: [] as IMessage[],
    };
  },
  computed: {
    currentMessages(): object[] {
      return this.messages.filter((mes: any) => {
        return (
          (mes.fromId == this.interlocutorId && mes.toId == this.myId) ||
          (mes.fromId == this.myId && mes.toId == this.interlocutorId)
        );
      });

      // const a = this.messages.filter((mes: any) => {
      //   return (
      //     (mes.fromId == this.interlocutorId && mes.toId == this.myId) ||
      //     (mes.fromId == this.myId && mes.toId == this.interlocutorId)
      //   );
      // });
      // for (let index = 0; index < 30; index++) {
      //   a.push(a[0]);
      // }
      // return a;
    },
  },
  async created() {
    this.myId = +(Cookie.getCookie("userId") || "");

    const promises = [];
    promises.push(Profile.model);
    promises.push(Data.getQuery("message", { from: this.myId || "" }));
    promises.push(Data.getQuery("message", { to: this.myId || "" }));
    promises.push(Data.getQuery("person"));

    const [profile, inputMessages, outputMessages, persons] = await Promise.all(
      promises
    );
    this.profile = profile;

    const allMessages = inputMessages;
    outputMessages.forEach((mes: any) => {
      allMessages.push(mes);
    });

    const ids = new Set();
    allMessages.forEach((mes: any) => {
      ids.add(mes.fromId == this.myId ? mes.toId : mes.fromId);
    });

    this.allInterlocutors = Array.from(ids).map((id) =>
      persons.find((person: any) => person.userId === id)
    );

    this.interlocutorId = this.allInterlocutors[0].userId;
    this.messages = allMessages;

    // флаг для отрисовки всего
    this.loaded = true;
  },
  watch: {},
  methods: {
    onTabChange() {
      this.interlocutorId = (this.allInterlocutors[this.tabId] as any).userId;
    },

    getDateText(dateISO: string): string {
      const date = new Date(dateISO);
      return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
    },
  },
});
</script>

<style>
.width100 {
  width: 100%;
}

.person__item {
  gap: 8px;
  width: 320px;
}

.person__name {
  text-overflow: ellipsis;
}

.interlocutor__avatar {
  width: 48px;
}

.interlocutor__text {
  max-width: 400px;
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
