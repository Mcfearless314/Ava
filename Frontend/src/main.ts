import { importProvidersFrom } from '@angular/core';
import {provideHttpClient, withInterceptors} from "@angular/common/http";
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule, bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import {authInterceptor} from "./app/interceptor/auth.interceptor";
import {provideRouter} from "@angular/router";
import {appRoutes} from "./app/app.routes";

bootstrapApplication(AppComponent, {
    providers: [provideRouter(appRoutes),importProvidersFrom(BrowserModule, ReactiveFormsModule), provideHttpClient(withInterceptors([authInterceptor]))]
})
  .catch(err => console.error(err));
