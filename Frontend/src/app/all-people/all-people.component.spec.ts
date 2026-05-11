import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllPeopleComponent } from './all-people.component';

describe('AllPeopleComponent', () => {
  let component: AllPeopleComponent;
  let fixture: ComponentFixture<AllPeopleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllPeopleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllPeopleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
