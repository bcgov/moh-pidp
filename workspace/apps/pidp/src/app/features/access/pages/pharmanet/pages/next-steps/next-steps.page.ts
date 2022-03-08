import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-next-steps',
  templateUrl: './next-steps.page.html',
  styleUrls: ['./next-steps.page.scss'],
})
export class NextStepsPage implements OnInit {
  public title: string;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }

  public onSubmit(): void {}

  public ngOnInit(): void {}
}
