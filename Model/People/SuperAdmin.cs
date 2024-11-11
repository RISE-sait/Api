using Api.Interfaces;

namespace Api.Model.People
{
    public class SuperAdmin(string name, string email): Account(name, email), IAdmin, ICoach
    {
        public void AddAdmin(string name) {

        }

        public void RemoveAdmin(string name) {
            
        }

        public void UpdateAdmin(string name) {
            
        }
    }
}