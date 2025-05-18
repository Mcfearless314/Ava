import { Component } from '@angular/core';
import { HomePageComponent } from './components/home-page/home-page.component';
import { HttpClientModule} from "@angular/common/http";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    standalone: true,
    imports: [HomePageComponent, HttpClientModule]
})
export class AppComponent {
  title = 'Ava';
}
