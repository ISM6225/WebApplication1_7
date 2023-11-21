using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1_7.DataAccess;
using WebApplication1_7.Models;

namespace WebApplication1_7.Controllers;

public class HomeController : Controller
{
    SchoolDbContext dbContext;
    public HomeController(SchoolDbContext context)
    {
        dbContext = context;
    }

    public IActionResult Index()
    {
        populateData();

        return View();
    }

    public async Task<IActionResult> Queries()
    {
        // run a few queries here

        return View();
    }

    void populateData()
    {
        Random rnd = new Random();

        string[] Colleges = {"Muma College of Business, MCOB",
                "College of Engineering, CoE", "College of Arts and Sciences, CAS"};
        string[] Courses = {"ISM 6225, Distributed Information Systems",
                "ISM 6218, Advanced Database Management Systems",
                "ISM 6328, Information Security and IT Risk Management"};
        string[] Students = { "Yugo First", "Percy Ware", "Try Out" };
        string[] Grades = { "A", "A-", "B+", "B", "B-" };

        College[] colleges = new College[Colleges.Length];
        Course[] courses = new Course[Courses.Length];
        Student[] students = new Student[Students.Length];

        for (int i = 0; i < Colleges.Length; i++)
        {
            College college = new College
            {
                Name = Colleges[i].Split(",")[0],
                Abbreviation = Colleges[i].Split(",")[1]
            };

            dbContext.Colleges.Add(college);
            colleges[i] = college;
        }

        for (int i = 0; i < Courses.Length; i++)
        {
            Course course = new Course
            {

                Number = Courses[i].Split(",")[0],
                Name = Courses[i].Split(",")[1],
                College = colleges[rnd.Next(colleges.Length)]
            };

            dbContext.Courses.Add(course);
            courses[i] = course;
        }

        for (int i = 0; i < Students.Length; i++)
        {
            Student student = new Student
            {
                Name = Students[i]
            };

            dbContext.Students.Add(student);
            students[i] = student;
        }

        foreach (Student student in students)
        {
            foreach (Course course in courses)
            {
                Enrollment enrollment = new Enrollment
                {
                    Course = course,
                    Student = student,
                    Grade = Grades[rnd.Next(Grades.Length)]
                };

                dbContext.Enrollments.Add(enrollment);
            }
        }

        dbContext.SaveChanges();
    }
}
