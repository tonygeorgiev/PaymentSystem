import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportcsvdialogComponent } from './importcsvdialog.component';

describe('ImportcsvdialogComponent', () => {
  let component: ImportcsvdialogComponent;
  let fixture: ComponentFixture<ImportcsvdialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImportcsvdialogComponent]
    });
    fixture = TestBed.createComponent(ImportcsvdialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
