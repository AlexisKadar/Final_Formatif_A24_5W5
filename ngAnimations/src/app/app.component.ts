import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, shake, shakeX, tada } from 'ng-animate';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  animations: [
    trigger('shake', [
      transition('* => shake', useAnimation(shake, { params: { timing: 2 } }))
    ]),
    trigger('bounce', [
      transition('* => bounce', useAnimation(bounce, { params: { timing: 4 } }))
    ]),
    trigger('tada', [
      transition('* => tada', useAnimation(tada, { params: { timing: 3 } }))
    ])
  ]
})
export class AppComponent {
  title = 'ngAnimations';

  carreRouge: string | null = null;
  carreVert: string | null = null;
  carreBleu: string | null = null;

  rotate = false;

  faireTourner() {
    this.rotate = true;

    setTimeout(() => {
      this.rotate = false;
    }, 2000);
  }

  boucle: boolean = false;

  animerUneFois() {
    this.boucle = false;
    this.animation();
  }


  animation() {
    this.carreRouge = null;
    this.carreVert = null;
    this.carreBleu = null;


    setTimeout(() => {
      this.carreRouge = 'shake';
    }, 0);

    setTimeout(() => {
      this.carreVert = 'bounce';
    }, 2000);

    setTimeout(() => {
      this.carreBleu = 'tada';
    }, 5000);

    if (this.boucle) {
      setTimeout(() => {
        this.animation();
      }, 8000);
    }

  }


  animerEnBoucle() {
    this.boucle = true;
    this.animation();
  }

}
