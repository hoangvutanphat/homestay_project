import HeroBanner from "@/components/HeroBanner";
import NewsletterSection from "@/components/NewsletterSection";
import PopularDestinations from "@/components/PopularDestinations";
import SpecialOffers from "@/components/SpecialOffers";
import Testimonials from "@/components/Testimonials";
import WhyChooseUs from "@/components/WhyChooseUs";

const Index = () => {
  return (
    <div className="min-h-screen flex flex-col bg-gradient-to-b from-slate-50 to-white">
      <main className="flex-1 flex flex-col items-center">
        <HeroBanner />
        <div className="container px-4 relative">{/* <SearchBox /> */}</div>
        <div className="bg-gradient-to-r from-white to-blue-50">
          <SpecialOffers />
        </div>
        <div className="bg-gradient-to-br from-gray-50 via-white to-blue-50">
          <PopularDestinations />
        </div>
        <div className="bg-gradient-to-r from-gray-50 to-blue-50">
          <WhyChooseUs />
        </div>
        <div className="bg-gradient-to-br from-white to-gray-50">
          <Testimonials />
        </div>
        <div className="bg-gradient-to-r from-blue-50 to-indigo-100">
          <NewsletterSection />
        </div>
      </main>
    </div>
  );
};

export default Index;
