import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { VerificadorIdadeComponent } from './verificador-idade/verificador-idade.component';

@Component({
  selector: 'app-root',
  imports: [FormsModule, VerificadorIdadeComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  idade = 0;
}
