<template>
  <div
    v-if="loaded"
    class="
      is-flex is-flex-wrap-nowrap is-justify-content-space-around
      mb-5
      mt-5
    "
  >
    <div class="card Profile__menu">
      <b-tabs vertical :animated="false" type="is-boxed">
        <b-tab-item label="О себе">
          <b-field label="Фамилия">
            <b-input placeholder="Введите вашу фамилию" v-model="profile.surname"></b-input>
          </b-field>
          <b-field label="Имя">
            <b-input placeholder="Введите ваше имя" v-model="profile.name"></b-input>
          </b-field>
          <b-field label="Отчество">
            <b-input placeholder="Введите ваше отчество" v-model="profile.patronymic"></b-input>
          </b-field>
          <b-field label="Пол">
            <b-radio v-model="radio" name="name" native-value="Мужской">
              Мужской
            </b-radio>
            <b-radio v-model="radio" name="name" native-value="Женский">
              Женский
            </b-radio>
          </b-field>
          <b-field label="Дата рождения">
            <b-datepicker
              position="is-top-left"
              v-model="selectedDate"
              :show-week-number="showWeekNumber"
              :locale="locale"
              placeholder="Введите вашу дату рождения"
              icon="calendar"
              icon-pack="fas"
              :icon-right="selectedDate ? 'times-circle' : ''"
              icon-right-clickable
              @icon-right-click="clearDate"
              trap-focus
            >
            </b-datepicker>
          </b-field>
          <b-button type="is-danger">Сохранить</b-button>
        </b-tab-item>
        <b-tab-item label="Местожительство">
          <b-field label="Страна">
            <b-select
              expanded
              v-model="type"
              placeholder="Выберите свою страну"
            >
              <option>Россия</option>
            </b-select>
          </b-field>
          <b-field label="Город">
            <b-select expanded v-model="type" placeholder="Выберите свой город">
              <option>Туймазы</option>
            </b-select>
          </b-field>
          <b-button type="is-danger">Сохранить</b-button>
        </b-tab-item>
        <b-tab-item label="Мои друзья">
          <b-field label="Друзья">
            <div class="notification is-flex is-justify-content-space-between">
              <div class="is-flex is-flex-wrap-nowrap person__group">
                <div
                  v-for="friend in friends"
                  v-bind:key="friend.id"
                  class="
                    person
                    is-flex is-justify-content-center is-flex-wrap-wrap
                  "
                >
                  <figure class="image is-48x48">
                    <img src="@/assets/person.png" />
                  </figure>
                  <p class="person__name is-size-7" :title="friend.name">{{ friend.name }}</p>
                </div>
              </div>
              <b-button type="is-primary is-align-self-flex-end"
                >Показать еще</b-button
              >
            </div>
          </b-field>
          <b-field label="Подписчики">
            <div class="notification is-flex is-justify-content-space-between">
              <div class="is-flex is-flex-wrap-nowrap person__group">
                <div
                  v-for="subscriber in subscribers"
                  v-bind:key="subscriber.id"
                  class="
                    person
                    is-flex is-justify-content-center is-flex-wrap-wrap
                  "
                >
                  <figure class="image is-48x48">
                    <img src="@/assets/person.png" />
                  </figure>
                  <p class="person__name is-size-7" :title="subscriber.name">{{ subscriber.name }}</p>
                </div>
              </div>
              <b-button type="is-primary is-align-self-flex-end"
                >Показать еще</b-button
              >
            </div>
          </b-field>
          <b-field label="Подписки">
            <div class="notification is-flex is-justify-content-space-between">
              <div class="is-flex is-flex-wrap-nowrap person__group">
                <div
                  v-for="subscription in subscriptions"
                  v-bind:key="subscription.id"
                  class="
                    person
                    is-flex is-justify-content-center is-flex-wrap-wrap
                  "
                >
                  <figure class="image is-48x48">
                    <img src="@/assets/person.png" />
                  </figure>
                  <p class="person__name is-size-7" :title="subscription.name">{{ subscription.name }}</p>
                </div>
              </div>
              <b-button type="is-primary is-align-self-flex-end"
                >Показать еще</b-button
              >
            </div>
          </b-field>
        </b-tab-item>
        <b-tab-item label="Интересы">
          <b-field label="Мои интересы">
            <b-taginput
              v-model="selectedTags"
              ellipsis
              icon="angle-right"
              icon-pack="fas"
              :data="filteredTags"
              autocomplete
              field="title"
              placeholder="Введите свои интересы"
              aria-close-label="Удалить интерес"
              @typing="getFilteredTags"
            >
            </b-taginput>
          </b-field>
          <b-button type="is-danger">Сохранить</b-button>
        </b-tab-item>
      </b-tabs>
    </div>
    <div class="card Profile__cards">
      <div class="is-flex is-justify-content-center">
        <b-image
          class="Profile__avatar"
          :src="require('@/assets/person.png')"
          alt="avatar"
        ></b-image>
      </div>
      <div class="card-content">
        <div class="media">
          <div class="media-content is-flex is-justify-content-center">
            <p class="title is-4">ФИО</p>
          </div>
        </div>
        <div
          class="content is-flex is-flex-direction-column is-align-items-center"
        >
          <span>Страна - Город</span>
          <span>Пол</span>
          <span>Возраст</span>
          <span>Интересы</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Profile from "../utils/Profile";
import Data from "../utils/Data";
import Cookie from "@/utils/Cookie";

interface IState {
  selectedDate?: Date | null;
  [x: string]: any;
}

interface IInterest {
  id: number;
  title: string;
}

export default Vue.extend({
  data() {
    return {
      // флаг загрузки
      loaded: false,
      profile: undefined,

      selectedDate: new Date(),
      showWeekNumber: false,
      locale: undefined, // Browser localeS
      filteredTags: undefined,
      selectedTags: [],
      tags: undefined,
      friends: [
        // {
        //   id: 0,
        //   surname: "Иванов",
        //   name: "Иван",
        //   patronymic: "Иванович",
        //   img: "`@/assets/person.png`",
        // },
        // {
        //   id: 1,
        //   surname: "Петров",
        //   name: "Петр",
        //   patronymic: "Петрович",
        //   img: "`@/assets/person.png`",
        // },
      ],
      subscribers: [
        // {
        //   id: 0,
        //   surname: "Викторов",
        //   name: "Виктор",
        //   patronymic: "Викторович",
        //   img: "@/assets/person.png",
        // },
        // {
        //   id: 1,
        //   surname: "Александров",
        //   name: "Алексей",
        //   patronymic: "Александрович",
        //   img: "@/assets/person.png",
        // },
        // {
        //   id: 2,
        //   surname: "Иванов",
        //   name: "Иван",
        //   patronymic: "Иванович",
        //   img: "@/assets/person.png",
        // },
      ],
      subscriptions: [
        // {
        //   id: 0,
        //   surname: "Викторов",
        //   name: "Виктор",
        //   patronymic: "Викторович",
        //   img: "@/assets/person.png",
        // },
        // {
        //   id: 1,
        //   surname: "Александров",
        //   name: "Алексей",
        //   patronymic: "Александрович",
        //   img: "@/assets/person.png",
        // },
        // {
        //   id: 2,
        //   surname: "Иванов",
        //   name: "Иван",
        //   patronymic: "Иванович",
        //   img: "@/assets/person.png",
        // },
      ],
    } as IState;
  },
  async created() {
    const myId = Cookie.getCookie('userId');

    const promises = [];
    promises.push(Profile.model);
    promises.push(Data.getQuery('friendship/' + myId));

    const [profile, friendship] = await Promise.all(promises);
    this.profile = profile;
    if (profile.birthDate) {
      this.selectedDate = new Date(profile.birthDate)
    }
    
    this.tags = profile.interests 
    // [
    //   { id: 0, title: "Книги" },
    //   { id: 1, title: "Кинофильмы" },
    //   { id: 2, title: "Музыка" },
    // ];
    this.filteredTags = this.tags;    

    this.friends = friendship.filter((f: any) => f.direction == 2)
      .map((f: any) => f.firstId == myId ? f.second : f.first);

    this.subscribers = friendship.filter((f: any) => f.firstId == myId && f.direction == 1 || f.secondId == myId && f.direction == 0)
      .map((f: any) => f.firstId == myId ? f.second : f.first);

    this.subscriptions = friendship.filter((f: any) => f.firstId == myId && f.direction == 0 || f.secondId == myId && f.direction == 1)
      .map((f: any) => f.firstId == myId ? f.second : f.first);

    // флаг для отрисовки всего
    this.loaded = true;
  },
  watch: {
    
  },
  methods: {
    clearDate(): void {
      this.selectedDate = null;
    },
    getFilteredTags(text: string) {
      this.filteredTags = this.tags.filter((interest: IInterest) => {
        return (
          !this.selectedTags.some(
            (selected: IInterest) => interest.id === selected.id
          ) && interest.title.toLowerCase().indexOf(text.toLowerCase()) >= 0
        );
      });
    },
  },
});
</script>

<style>
.Profile__menu {
  width: 50%;
}

.Profile__cards {
  width: 40%;
  padding-top: 38px;
}

.Profile__avatar {
  width: 250px;
}

.person__group {
  gap: 30px;
}

.person__name {
  overflow: hidden;
  text-overflow: ellipsis;
}

.person {
  width: 48px;
}
</style>
