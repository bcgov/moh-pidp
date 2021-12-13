import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { selfDeclarationQuestions } from './self-declaration-questions';
import { SelfDeclarationTypeEnum } from './self-declaration.enum';

@Component({
  selector: 'app-self-declaration',
  templateUrl: './self-declaration.component.html',
  styleUrls: ['./self-declaration.component.scss'],
})
export class SelfDeclarationComponent implements OnInit {
  public title: string;

  public SelfDeclarationTypeEnum = SelfDeclarationTypeEnum;
  public selfDeclarationQuestions = selfDeclarationQuestions;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }

  public ngOnInit(): void {}
}
