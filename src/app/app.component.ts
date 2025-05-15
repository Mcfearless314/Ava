import { Component } from '@angular/core';
import { HomePageComponent } from './components/home-page/home-page.component';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    standalone: true,
    imports: [HomePageComponent]
})
export class AppComponent {
  title = 'Ava';
}
