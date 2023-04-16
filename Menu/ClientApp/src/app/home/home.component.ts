import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor() {
    const base = document.querySelector('base');
    const href = base && base.href ? base.href : '';
    // set base href for Angular router
    (window as any).__webpack_public_path__ = href;
  }
}
