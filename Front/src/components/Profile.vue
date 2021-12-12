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
            <b-input
              placeholder="Введите вашу фамилию"
              v-model="profile.surname"
            ></b-input>
          </b-field>
          <b-field label="Имя">
            <b-input
              placeholder="Введите ваше имя"
              v-model="profile.name"
            ></b-input>
          </b-field>
          <b-field label="Отчество">
            <b-input
              placeholder="Введите ваше отчество"
              v-model="profile.patronymic"
            ></b-input>
          </b-field>
          <b-field label="Пол">
            <b-radio v-model="profile.genderId" name="name" native-value="1">
              Мужской
            </b-radio>
            <b-radio v-model="profile.genderId" name="name" native-value="2">
              Женский
            </b-radio>
          </b-field>
          <b-field label="Дата рождения">
            <b-datepicker
              position="is-top-left"
              v-model="selectedDate"
              :show-week-number="showWeekNumber"
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
        </b-tab-item>
        <b-tab-item label="Местожительство">
          <b-field label="Город">
            <b-select
              expanded
              v-model="profile.cityId"
              placeholder="Выберите свой город"
            >
              <option
                v-for="city in cities"
                v-bind:key="city.id"
                :value="city.id"
              >
                {{ `${city.title} (${city.country.title})` }}
              </option>
            </b-select>
          </b-field>
        </b-tab-item>
        <b-tab-item label="Мои друзья">
          <b-field label="Друзья">
            <div class="notification is-flex is-justify-content-space-between">
              <div class="is-flex is-flex-wrap-nowrap person__group">
                <div v-if="!friends.length">Друзей нет</div>
                <div
                  v-for="friend in friendsCrop"
                  v-bind:key="friend.id"
                  class="
                    person
                    is-flex is-justify-content-center is-flex-wrap-wrap
                  "
                >
                  <figure class="image is-48x48">
                    <img src="@/assets/person.png" />
                  </figure>
                  <p class="person__name is-size-7" :title="friend.name">
                    {{ friend.name }}
                  </p>
                </div>
              </div>
              <b-button
                v-if="friends.length > 4"
                type="is-primary is-align-self-flex-end"
                @click="showMoreFriends('friends')"
                >Показать еще</b-button
              >
            </div>
          </b-field>
          <b-field label="Подписчики">
            <div class="notification is-flex is-justify-content-space-between">
              <div class="is-flex is-flex-wrap-nowrap person__group">
                <div v-if="!subscribers.length">Подписчиков нет</div>
                <div
                  v-for="subscriber in subscribersCrop"
                  v-bind:key="subscriber.id"
                  class="
                    person
                    is-flex is-justify-content-center is-flex-wrap-wrap
                  "
                >
                  <figure class="image is-48x48">
                    <img src="@/assets/person.png" />
                  </figure>
                  <p class="person__name is-size-7" :title="subscriber.name">
                    {{ subscriber.name }}
                  </p>
                </div>
              </div>
              <b-button
                v-if="subscribers.length > 4"
                type="is-primary is-align-self-flex-end"
                @click="showMoreFriends('subscribers')"
                >Показать еще</b-button
              >
            </div>
          </b-field>
          <b-field label="Подписки">
            <div class="notification is-flex is-justify-content-space-between">
              <div class="is-flex is-flex-wrap-nowrap person__group">
                <div v-if="!subscriptions.length">Подписок нет</div>
                <div
                  v-for="subscription in subscriptionsCrop"
                  v-bind:key="subscription.id"
                  class="
                    person
                    is-flex is-justify-content-center is-flex-wrap-wrap
                  "
                >
                  <figure class="image is-48x48">
                    <img src="@/assets/person.png" />
                  </figure>
                  <p class="person__name is-size-7" :title="subscription.name">
                    {{ subscription.name }}
                  </p>
                </div>
              </div>
              <b-button
                v-if="subscriptions.length > 4"
                type="is-primary is-align-self-flex-end"
                @click="showMoreFriends('subscriptions')"
                >Показать еще</b-button
              >
            </div>
          </b-field>
        </b-tab-item>
        <b-tab-item label="Интересы">
          <b-field label="Мои интересы">
            <b-taginput
              v-model="profile.interests"
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
            <p class="title is-4">
              {{ profile.surname }} {{ profile.name }} {{ profile.patronymic }}
            </p>
          </div>
        </div>
        <div
          class="content is-flex is-flex-direction-column is-align-items-center"
        >
          <span v-if="this.currentCity">
            {{ this.currentCity.country.title }} -
            {{ this.currentCity.title }}</span
          >
          <span v-if="profile.genderId">{{
            profile.genderId == 2 ? "Пол - женский" : "Пол - мужской"
          }}</span>
          <span v-if="this.selectedDate"
            >Возраст - {{ ageToStr(getAge(this.selectedDate)) }}</span
          >
          <div
            class="
              content__interests
              is-flex
              is-flex-direction-column
              is-align-items-center
              is-flex-wrap-wrap
            "
          >
            <div v-if="profile.interests && profile.interests.length">
              Интересы:
            </div>
            <div class="interests__interest is-flex is-justify-content-center">
              <p v-for="interest in profile.interests" v-bind:key="interest.id">
                {{ interest.title }}
              </p>
            </div>
          </div>
        </div>
      </div>
      <b-button type="is-danger" class="m-5" @click="save">Сохранить</b-button>
    </div>
    <b-modal v-model="isModalActive">
      <div
        class="
          card
          is-flex
          is-flex-wrap-nowrap
          is-justify-content-center
          is-flex-wrap-wrap
          p-4
        "
      >
        <div
          v-for="friend in typeModalActive"
          v-bind:key="friend.id"
          class="person__group is-flex is-flex-wrap-nowrap p-2"
        >
          <figure class="image is-128x128 is-flex is-align-self-center">
            <img src="@/assets/person.png" />
          </figure>
          <div>
            <p class="person__name person__name-size is-align-self-center">
              {{ friend.surname }} {{ friend.name }}
              {{ friend.patronymic }}
            </p>
            <b-field label="Интересы">
              <b-taginput
                :value="friend.interests"
                ellipsis
                field="title"
                :closable="false"
                :readonly="true"
              >
              </b-taginput>
            </b-field>
             <p v-if="friend.cityId">{{friend.city.title}}</p>
          </div>
        </div>
      </div>
    </b-modal>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Profile from "../utils/Profile";
import Data from "../utils/Data";
import Cookie from "@/utils/Cookie";
import { ToastProgrammatic as Toast } from "buefy";

interface IInterest {
  id: number;
  title: string;
}

export default Vue.extend({
  data() {
    return {
      // флаг загрузки
      loaded: false,
      profile: undefined as any,
      cities: [] as any[],
      selectedDate: null as Date | null,
      showWeekNumber: false,
      filteredTags: [] as any[],
      tags: [] as any[],
      friends: [] as any[],
      subscribers: [] as any[],
      subscriptions: [] as any[],
      isModalActive: false,
      typeModalActive: [] as any[],
    };
  },
  computed: {
    currentCity(): object | undefined {
      if (!this.profile?.cityId) {
        return undefined;
      }

      return this.cities.find((city: any) => city.id == this.profile.cityId);
    },
    friendsCrop() {
      let index = 0;
      const result = [] as any[];
      for (let i = 0; i < this.friends.length; i++) {
        if (index < 4) {
          index++;
          result.push(this.friends[i]);
        } else {
          break;
        }
      }
      return result;
    },
    subscribersCrop() {
      let index = 0;
      const result = [] as any[];
      for (let i = 0; i < this.subscribers.length; i++) {
        if (index < 4) {
          index++;
          result.push(this.subscribers[i]);
        } else {
          break;
        }
      }
      return result;
    },
    subscriptionsCrop() {
      let index = 0;
      const result = [] as any[];
      for (let i = 0; i < this.subscriptions.length; i++) {
        if (index < 4) {
          index++;
          result.push(this.subscriptions[i]);
        } else {
          break;
        }
      }
      return result;
    },
  },
  async created() {
    const myId = Cookie.getCookie("userId");

    const promises = [];
    promises.push(Profile.model);
    promises.push(Data.getQuery("friendship/" + myId));
    promises.push(Data.getQuery("interest"));
    promises.push(Data.getQuery("city"));

    const [profile, friendship, allInterests, allcities] = await Promise.all(
      promises
    );
    this.profile = profile;
    if (profile.birthDate) {
      this.selectedDate = new Date(profile.birthDate);
    }

    this.tags = allInterests;
    this.cities = allcities;

    this.friends = friendship
      .filter((f: any) => f.direction == 2)
      .map((f: any) => (f.firstId == myId ? f.second : f.first));

    // убрать
    for (let index = 0; index < 4; index++) {
      this.friends = this.friends.concat(this.friends);
    }

    this.subscribers = friendship
      .filter(
        (f: any) =>
          (f.firstId == myId && f.direction == 1) ||
          (f.secondId == myId && f.direction == 0)
      )
      .map((f: any) => (f.firstId == myId ? f.second : f.first));

    this.subscriptions = friendship
      .filter(
        (f: any) =>
          (f.firstId == myId && f.direction == 0) ||
          (f.secondId == myId && f.direction == 1)
      )
      .map((f: any) => (f.firstId == myId ? f.second : f.first));

    // флаг для отрисовки всего
    this.loaded = true;
  },
  watch: {
    selectedDate(newValue: Date) {
      this.profile.birthDate = newValue.toISOString();
    },
  },
  methods: {
    save() {
      const myId = Cookie.getCookie("userId");
      Data.putQuery("person/" + myId, this.profile)
        .then(() => {
          Toast.open({
            message: "Сохранение прошло успешно",
            type: "is-success",
          });
        })
        .catch(() => {
          Toast.open({ message: "Сохранение не удалось", type: "is-danger" });
        });
    },
    clearDate(): void {
      this.selectedDate = null;
    },
    getFilteredTags(text: string) {
      this.filteredTags = this.tags.filter((interest: IInterest) => {
        return (
          !this.profile.interests?.some(
            (selected: IInterest) => interest.id === selected.id
          ) && interest.title.toLowerCase().indexOf(text.toLowerCase()) >= 0
        );
      });
    },
    getAge(dateISO: string): string {
      const yearBirth = new Date(dateISO).getFullYear();
      const year = new Date().getFullYear();
      let age = (year - yearBirth).toString();

      return age;
    },

    ageToStr(age: string): string {
      let txt;
      let count = Number(age) % 100;
      if (count >= 5 && count <= 20) {
        txt = "лет";
      } else {
        count = count % 10;
        if (count == 1) {
          txt = "год";
        } else if (count >= 2 && count <= 4) {
          txt = "года";
        } else {
          txt = "лет";
        }
      }
      return age + " " + txt;
    },
    showMoreFriends(type: "friends" | "subscriptions" | "subscribers") {
      this.isModalActive = true;
      switch (type) {
        case "friends":
          this.typeModalActive = this.friends;
          break;
        case "subscriptions":
          this.typeModalActive = this.subscriptions;
          break;
        case "subscribers":
          this.typeModalActive = this.subscribers;
          break;
      }
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
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.person {
  width: 48px;
}

.content__interests,
.interests__interest {
  width: 100%;
}
.interests__interest {
  gap: 10px;
}
</style>
