import { Component, input } from '@angular/core';

@Component({
  selector: 'app-verificador-idade',
  templateUrl: './verificador-idade.component.html',
  styleUrl: './verificador-idade.component.css'
})
export class VerificadorIdadeComponent {
  idade = input<number>(0);
}
