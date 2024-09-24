import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestWebclientComponent } from './test-webclient.component';

describe('TestWebclientComponent', () => {
  let component: TestWebclientComponent;
  let fixture: ComponentFixture<TestWebclientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TestWebclientComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestWebclientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
