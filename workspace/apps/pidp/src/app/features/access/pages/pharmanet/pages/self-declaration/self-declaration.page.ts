import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { selfDeclarationQuestions } from './self-declaration-questions';
import { SelfDeclarationType } from './self-declaration.enum';

@Component({
  selector: 'app-self-declaration',
  templateUrl: './self-declaration.page.html',
  styleUrls: ['./self-declaration.page.scss'],
})
export class SelfDeclarationPage implements OnInit {
  public title: string;

  public SelfDeclarationType = SelfDeclarationType;
  public selfDeclarationQuestions = selfDeclarationQuestions;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }

  public ngOnInit(): void {}
}
