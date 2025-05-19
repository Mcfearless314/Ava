import { importProvidersFrom } from '@angular/core';
import { provideHttpClient } from "@angular/common/http";
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule, bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';

bootstrapApplication(AppComponent, {
    providers: [importProvidersFrom(BrowserModule, ReactiveFormsModule), provideHttpClient()]
})
  .catch(err => console.error(err));
