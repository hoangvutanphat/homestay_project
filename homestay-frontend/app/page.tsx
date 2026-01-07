import HeroBanner from "@/components/HeroBanner";
// import FeaturedProperties from "@/components/FeaturedProperties";
import Footer from "@/components/Footer";
import NewsletterSection from "@/components/NewsletterSection";
import SpecialOffers from "@/components/SpecialOffers";
import Testimonials from "@/components/Testimonials";
import WhyChooseUs from "@/components/WhyChooseUs";
import PopularDestinations from "@/components/PopularDestinations";

const Index = () => {
  return (
    <div className="min-h-screen flex flex-col bg-gradient-to-b from-slate-50 to-white">
      <main className="flex-1 flex flex-col items-center">
        <HeroBanner />

        <div className="container px-4 relative">{/* <SearchBox /> */}</div>
        {/* <div className="bg-gradient-to-br from-blue-50 via-white to-indigo-50">
          <FeaturedProperties />
        </div> */}
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
      <Footer />
    </div>
  );
};

export default Index;
