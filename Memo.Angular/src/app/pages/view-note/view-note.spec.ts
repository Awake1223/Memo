import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewNote } from './view-note';

describe('ViewNote', () => {
  let component: ViewNote;
  let fixture: ComponentFixture<ViewNote>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ViewNote]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewNote);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
